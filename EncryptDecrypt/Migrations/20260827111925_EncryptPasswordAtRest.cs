using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EncryptDecrypt.Migrations
{
    /// <inheritdoc />
    public partial class EncryptPasswordAtRest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PDFPassword",
                table: "PasswordHint",
                type: "BLOB",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 25);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PDFPassword",
                table: "PasswordHint",
                type: "TEXT",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldMaxLength: 512);
        }
    }
}
