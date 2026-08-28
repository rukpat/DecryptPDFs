using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class PasswordManagerContext : DbContext
{
    public DbSet<PasswordManagerEntity> PasswordHint { get; set; }

    private string gDBPath = string.Empty;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // %LocalAppData%\PDFTools\PDFTools.db
        // Store under the Windows user profile (not the app's install/build folder) so the file
        // itself is only accessible to this Windows account, matching the DPAPI protection below
        // (which ties decryption to the same account). 
        var dbDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PDFTools");
        var dbPath = Path.Combine(dbDirectory, "PDFTools.db");

        // Ensure the directory exists
        if (!Directory.Exists(dbDirectory))
        {
            Directory.CreateDirectory(dbDirectory);
        }

        gDBPath = dbPath;

        // Use the SQLite database at the specified path
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    public string GetDBPath()
    {
        return gDBPath;
    }

    // Returns every stored password, most-likely-to-work first: highest SuccessCount wins,
    // ties broken by whichever was used most recently (falling back to CreatedAt if never used)
    public List<PasswordManagerEntity> GetPasswordsByLikelihood()
    {
        return PasswordHint
            .AsEnumerable()
            .OrderByDescending(p => p.SuccessCount)
            .ThenByDescending(p => p.LastUsedAt ?? p.CreatedAt)
            .ToList();
    }

    // Fast path: caller already knows which stored row matched (e.g. from GetPasswordsByLikelihood),
    // so look it up by primary key rather than comparing password values
    public void RecordPasswordSuccess(int id)
    {
        var existing = PasswordHint.Find(id);
        if (existing != null)
        {
            existing.SuccessCount++;
            existing.LastUsedAt = DateTime.Now;
            SaveChanges();
        }
    }

    // Slow path: caller only has the raw password text (e.g. typed in manually), so we don't yet
    // know if it's a known row. Update the matching row if one exists, otherwise save it as new.
    public void RecordPasswordSuccess(string password)
    {
        if (string.IsNullOrEmpty(password)) return;

        // AsEnumerable() forces this to compare in memory (after decryption), not via SQL: DPAPI
        // encryption is non-deterministic, so the same plaintext never produces the same ciphertext
        // twice, and a server-side WHERE PDFPassword = @password would never match.
        var existing = PasswordHint.AsEnumerable().FirstOrDefault(p => p.PDFPassword == password);
        if (existing != null)
        {
            RecordPasswordSuccess(existing.ID);
        }
        else
        {
            // Brand-new password that worked - save it so future runs can try it automatically
            PasswordHint.Add(new PasswordManagerEntity
            {
                Nickname = "Auto-saved",
                Description = "Captured automatically from a successful decrypt.",
                PDFPassword = password,
                SuccessCount = 1,
                LastUsedAt = DateTime.Now
            });
            SaveChanges();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Encrypt PDFPassword at rest with DPAPI, tied to the current Windows account - nothing
        // else in the app changes: this only affects the bytes written to/read from the DB file,
        // so PDFPassword still holds plain text everywhere in memory (the CRUD grid, EnD.cs, the
        // [StringLength(25)] check above). Only in-process code running as this Windows user, on
        // this machine, can ever get the plaintext back out.
        var passwordConverter = new ValueConverter<string, byte[]>(
            plaintext => ProtectedData.Protect(Encoding.UTF8.GetBytes(plaintext), null, DataProtectionScope.CurrentUser),
            encrypted => Encoding.UTF8.GetString(ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser)));

        modelBuilder.Entity<PasswordManagerEntity>()
            .Property(p => p.PDFPassword)
            .HasConversion(passwordConverter)
            // Override the 25-char facet the [StringLength(25)] attribute would otherwise apply
            // to this column - that limit is meant for the plaintext, not the (longer) ciphertext.
            .HasMaxLength(512);
    }

    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries<PasswordManagerEntity>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.Now;
                entry.Entity.LastModified = DateTime.Now;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = DateTime.Now;
            }
        }

        return base.SaveChanges();
    }
}

public class PasswordManagerEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [StringLength(20, ErrorMessage = "Nickname cannot be longer than 20 characters.")]
    public string Nickname { get; set; } = string.Empty; // Initialize with default value

    [StringLength(200, ErrorMessage = "Description cannot be longer than 200 characters.")]
    public string Description { get; set; } = string.Empty; // Initialize with default value

    [Required(ErrorMessage = "PDFPassword is required.")]
    [StringLength(25, ErrorMessage = "PDFPassword cannot be longer than 25 characters.")]
    public string PDFPassword { get; set; } = string.Empty; // Initialize with default value

    public DateTime CreatedAt { get; set; }

    public DateTime LastModified { get; set; }

    public int SuccessCount { get; set; } = 0; // Times this password has successfully decrypted a file

    public DateTime? LastUsedAt { get; set; } // Null until it succeeds for the first time
}

public static class ValidationHelper
{
    public static bool TryValidateEntity(object entity, out List<ValidationResult> validationResults)
    {
        var context = new ValidationContext(entity, serviceProvider: null, items: null);
        validationResults = new List<ValidationResult>();
        return Validator.TryValidateObject(entity, context, validationResults, validateAllProperties: true);
    }
}
