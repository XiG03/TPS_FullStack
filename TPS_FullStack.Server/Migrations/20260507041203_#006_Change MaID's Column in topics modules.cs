using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS_FullStack.Server.Migrations
{
    /// <inheritdoc />
    public partial class _006_ChangeMaIDsColumnintopicsmodules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaId",
                table: "Chuyende_Dapan",
                newName: "MaID");

            migrationBuilder.RenameColumn(
                name: "ChuyendeId",
                table: "Chuyende_Cauhoi",
                newName: "ChuyendeID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaID",
                table: "Chuyende_Dapan",
                newName: "MaId");

            migrationBuilder.RenameColumn(
                name: "ChuyendeID",
                table: "Chuyende_Cauhoi",
                newName: "ChuyendeId");
        }
    }
}
