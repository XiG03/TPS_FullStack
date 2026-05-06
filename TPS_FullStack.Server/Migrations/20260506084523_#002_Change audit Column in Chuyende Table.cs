using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS_FullStack.Server.Migrations
{
    /// <inheritdoc />
    public partial class _002_ChangeauditColumninChuyendeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "Chuyende",
                type: "NVARCHAR(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Chuyende",
                type: "NVARCHAR(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Chuyende",
                type: "NVARCHAR(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UpdatedBy",
                table: "Chuyende",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DeletedBy",
                table: "Chuyende",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatedBy",
                table: "Chuyende",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(450)",
                oldNullable: true);
        }
    }
}
