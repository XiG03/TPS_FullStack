using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS_FullStack.Server.Migrations
{
    /// <inheritdoc />
    public partial class _007_AddtableChuyende_GiangvienLinkbothTopicsandTeachertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chuyende_Giangvien",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    ChuyendeID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    GiangvienID = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chuyende_Giangvien", x => x.MaID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chuyende_Giangvien");
        }
    }
}
