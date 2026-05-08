using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS_FullStack.Server.Migrations
{
    /// <inheritdoc />
    public partial class _007_AddRelationtoTopictable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Chuyende_Tailieu_ChuyendeID",
                table: "Chuyende_Tailieu",
                column: "ChuyendeID");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyende_Giangvien_ChuyendeID",
                table: "Chuyende_Giangvien",
                column: "ChuyendeID");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyende_Dapan_Chuyende_CauhoiID",
                table: "Chuyende_Dapan",
                column: "Chuyende_CauhoiID");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyende_Cauhoi_ChuyendeID",
                table: "Chuyende_Cauhoi",
                column: "ChuyendeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Chuyende_Cauhoi_Chuyende_ChuyendeID",
                table: "Chuyende_Cauhoi",
                column: "ChuyendeID",
                principalTable: "Chuyende",
                principalColumn: "MaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chuyende_Dapan_Chuyende_Cauhoi_Chuyende_CauhoiID",
                table: "Chuyende_Dapan",
                column: "Chuyende_CauhoiID",
                principalTable: "Chuyende_Cauhoi",
                principalColumn: "MaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chuyende_Giangvien_Chuyende_ChuyendeID",
                table: "Chuyende_Giangvien",
                column: "ChuyendeID",
                principalTable: "Chuyende",
                principalColumn: "MaID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chuyende_Tailieu_Chuyende_ChuyendeID",
                table: "Chuyende_Tailieu",
                column: "ChuyendeID",
                principalTable: "Chuyende",
                principalColumn: "MaID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chuyende_Cauhoi_Chuyende_ChuyendeID",
                table: "Chuyende_Cauhoi");

            migrationBuilder.DropForeignKey(
                name: "FK_Chuyende_Dapan_Chuyende_Cauhoi_Chuyende_CauhoiID",
                table: "Chuyende_Dapan");

            migrationBuilder.DropForeignKey(
                name: "FK_Chuyende_Giangvien_Chuyende_ChuyendeID",
                table: "Chuyende_Giangvien");

            migrationBuilder.DropForeignKey(
                name: "FK_Chuyende_Tailieu_Chuyende_ChuyendeID",
                table: "Chuyende_Tailieu");

            migrationBuilder.DropIndex(
                name: "IX_Chuyende_Tailieu_ChuyendeID",
                table: "Chuyende_Tailieu");

            migrationBuilder.DropIndex(
                name: "IX_Chuyende_Giangvien_ChuyendeID",
                table: "Chuyende_Giangvien");

            migrationBuilder.DropIndex(
                name: "IX_Chuyende_Dapan_Chuyende_CauhoiID",
                table: "Chuyende_Dapan");

            migrationBuilder.DropIndex(
                name: "IX_Chuyende_Cauhoi_ChuyendeID",
                table: "Chuyende_Cauhoi");
        }
    }
}
