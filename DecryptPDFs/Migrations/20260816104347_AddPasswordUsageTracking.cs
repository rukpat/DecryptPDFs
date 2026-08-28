using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DecryptPDFs.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordUsageTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilenameRegEx",
                table: "PasswordHint");

            migrationBuilder.DropColumn(
                name: "Foldername",
                table: "PasswordHint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "PasswordHint",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "PasswordHint",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUsedAt",
                table: "PasswordHint",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SuccessCount",
                table: "PasswordHint",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUsedAt",
                table: "PasswordHint");

            migrationBuilder.DropColumn(
                name: "SuccessCount",
                table: "PasswordHint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "PasswordHint",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "PasswordHint",
                type: "TEXT",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "FilenameRegEx",
                table: "PasswordHint",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Foldername",
                table: "PasswordHint",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
