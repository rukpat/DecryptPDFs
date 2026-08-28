using DecryptPDFs.Properties;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DecryptPDFs
{
    public partial class PasswordManagerForm : Form
    {
        private PasswordManagerContext dbContext;

        // Set once a save actually goes through - EnD checks this after ShowDialog() returns to
        // decide whether it's worth retrying stored passwords against the current file list.
        public bool PasswordsChanged { get; private set; }

        public PasswordManagerForm()
        {
            InitializeComponent();

            // Restore form size and position
            this.StartPosition = FormStartPosition.Manual;
            this.Width = Settings.Default.PMFormWidth;
            this.Height = Settings.Default.PMFormHeight;
            this.Left = Settings.Default.PMFormLeft;
            this.Top = Settings.Default.PMFormTop;
            // Restore window state
            if (Enum.TryParse(Properties.Settings.Default.PMFormWindowState, out FormWindowState windowState))
            {
                this.WindowState = windowState;
            }

            dbContext = new PasswordManagerContext();

            EnsureDatabaseCreated();
            LoadData();
        }

        private void EnsureDatabaseCreated()
        {
            dbContext.Database.Migrate();
        }
        private void LoadData()
        {
            dbContext.PasswordHint.Load();
            passwordManagerEntityBindingSource.DataSource = dbContext.PasswordHint.Local.ToBindingList();
        }
        // Passwords stay masked in the grid until "Show Passwords" is checked - only affects the
        // read-only display; double-clicking a cell to edit it still shows the real value, same as
        // any other cell.
        private void dataGridViewPWDMgr_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == PDFPassword.Index && !checkBoxShowPasswords.Checked && e.Value is string)
            {
                e.Value = "********";
                e.FormattingApplied = true;
            }
        }

        private void checkBoxShowPasswords_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewPWDMgr.Refresh();
        }

        private void dataGridViewPWDMgr_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {dataGridViewPWDMgr.SelectedRows.Count} row(s)?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateDataGridView())
            {
                dbContext.SaveChanges();
                dataGridViewPWDMgr.Refresh(); // Force DataGridView to refresh
                PasswordsChanged = true;
            }
        }
        private bool ValidateDataGridView()
        {
            var errorMessages = new StringBuilder();
            var firstErrorCell = (DataGridViewCell)null;

            foreach (DataGridViewRow row in dataGridViewPWDMgr.Rows)
            {
                if (row.IsNewRow) continue;

                var entity = row.DataBoundItem as PasswordManagerEntity;
                if (entity != null)
                {
                    if (!ValidationHelper.TryValidateEntity(entity, out List<ValidationResult> validationResults))
                    {
                        foreach (var validationResult in validationResults)
                        {
                            var memberName = validationResult.MemberNames.FirstOrDefault();
                            var cell = row.Cells[memberName];
                            if (firstErrorCell == null)
                            {
                                firstErrorCell = cell;
                            }
                            errorMessages.AppendLine($"Row {row.Index + 1}, Column {memberName}: {validationResult.ErrorMessage}\n");
                        }
                    }
                }
            }

            if (errorMessages.Length > 0)
            {
                // Copy error messages to clipboard
                Clipboard.SetText(errorMessages.ToString());

                errorMessages.AppendLine($"\n\nNOTE:This message is copied to the clipboard.");
                MessageBox.Show(errorMessages.ToString(), "Validation Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (firstErrorCell != null)
                {
                    dataGridViewPWDMgr.CurrentCell = firstErrorCell;
                    dataGridViewPWDMgr.BeginEdit(true);
                }
                return false;
            }

            return true;
        }
        private void linkLabelDBPath_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string filePath = dbContext.GetDBPath();
            string directoryPath = Path.GetDirectoryName(filePath) ?? string.Empty;

            if (Directory.Exists(directoryPath))
            {
                System.Diagnostics.Process.Start("explorer.exe", directoryPath);
            }
        }
        private void PasswordManagerForm_Load(object sender, EventArgs e)
        {
            string filePath = dbContext.GetDBPath();
            string fileName = Path.GetFileName(filePath);
            string directoryPath = Path.GetDirectoryName(filePath) ?? string.Empty;
            linkLabelDBPath.Text = fileName;
            toolTipDBPath.SetToolTip(this.linkLabelDBPath, "Click to open the folder:\n" + directoryPath);
        }
        private bool IsDataModified()
        {
            return dbContext.ChangeTracker.Entries()
                .Any(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
        }
        private void PasswordManagerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDataModified())
            {
                var result = MessageBox.Show("All unsaved data will be lost!\n\nDo you want to save your changes before closing?", "Data Changed", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
                if (result == DialogResult.Yes)
                {
                    dbContext.SaveChanges();
                }
                else if (result == DialogResult.No)
                {
                    dbContext.Dispose();
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // Cancel the closing event
                    return;
                }
            }
        }
        private void PasswordManagerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Save Form window state
            if (this.WindowState == FormWindowState.Normal)
            {
                Settings.Default.PMFormWidth = this.Width;
                Settings.Default.PMFormHeight = this.Height;
                Settings.Default.PMFormLeft = this.Left;
                Settings.Default.PMFormTop = this.Top;
            }
            else
            {
                Settings.Default.PMFormWidth = this.RestoreBounds.Width;
                Settings.Default.PMFormHeight = this.RestoreBounds.Height;
                Settings.Default.PMFormLeft = this.RestoreBounds.Left;
                Settings.Default.PMFormTop = this.RestoreBounds.Top;
            }

            Settings.Default.PMFormWindowState = this.WindowState.ToString();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
