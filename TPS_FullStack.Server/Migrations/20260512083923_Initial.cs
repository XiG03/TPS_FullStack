using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPS_FullStack.Server.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Kichhoat = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chungchi",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    KhoahocID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Ten = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Mota = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    Donvicap = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    Thoigiansudung = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Khongsudung = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chungchi", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Chungchi_Hocvien",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    KhoahocID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    ChungchiID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    HocvienID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Chungchi_Ten = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Chungchi_Mota = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    Chungchi_Donvicap = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Chungchi_Thoigiansudung = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Ngaycap = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ngayhethan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Khongsudung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chungchi_Hocvien", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Chuyende",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Ten = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Mota = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    Khongsudung = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chuyende", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Khoahoc",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Ten = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Mota = table.Column<string>(type: "NVARCHAR(500)", nullable: false),
                    Diemdat = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    ChungchiID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Khongsudung = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    Thu = table.Column<string>(type: "NVARCHAR(100)", nullable: false),
                    Thoigianhoc = table.Column<decimal>(type: "DECIMAL(4,1)", nullable: false),
                    Sobuoihoc = table.Column<decimal>(type: "DECIMAL(4,1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoahoc", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Khoahoc_Chuyende",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    KhoahocID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    ChuyendeID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    slCauhoi = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoahoc_Chuyende", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Khoahoc_DmTrangthai",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    KhoahocID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Ten = table.Column<string>(type: "NVARCHAR(200)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoahoc_DmTrangthai", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Khoahoc_Giangvien",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    KhoahocID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    GiangvienID = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoahoc_Giangvien", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Khoahoc_Hocvien",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    KhoahocID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    HocvienID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Diem = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Dieuchinh = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Khoahoc_Hocvien", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Lichhoc",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    LichhocID = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    GiangvienID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Ngay = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Tugio = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Dengio = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lichhoc", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Lichhoc_Giangvien_Diemdanh",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    LichhocID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    GiangvienID = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lichhoc_Giangvien_Diemdanh", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "Lichhoc_Hocvien_Diemdanh",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    LichhocID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    HocvienID = table.Column<string>(type: "NVARCHAR(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lichhoc_Hocvien_Diemdanh", x => x.MaID);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Giangvien",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hoten = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Ngaysinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gioitinh = table.Column<string>(type: "NVARCHAR(10)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Diachi = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Dienthoai = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    Khongsudung = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Giangvien", x => x.MaID);
                    table.ForeignKey(
                        name: "FK_Giangvien_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hocvien",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Hoten = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Ngaysinh = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gioitinh = table.Column<string>(type: "NVARCHAR(10)", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Diachi = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Dienthoai = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    CreatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    UpdatedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    DeletedBy = table.Column<string>(type: "NVARCHAR(450)", nullable: true),
                    Khongsudung = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hocvien", x => x.MaID);
                    table.ForeignKey(
                        name: "FK_Hocvien_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    refreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chuyende_Cauhoi",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    ChuyendeID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Ten = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Diem = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chuyende_Cauhoi", x => x.MaID);
                    table.ForeignKey(
                        name: "FK_Chuyende_Cauhoi_Chuyende_ChuyendeID",
                        column: x => x.ChuyendeID,
                        principalTable: "Chuyende",
                        principalColumn: "MaID",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    table.ForeignKey(
                        name: "FK_Chuyende_Giangvien_Chuyende_ChuyendeID",
                        column: x => x.ChuyendeID,
                        principalTable: "Chuyende",
                        principalColumn: "MaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chuyende_Tailieu",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    ChuyendeID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Tieude = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Ngaytao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Loaitailieu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kichthuoc = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Khongsudung = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chuyende_Tailieu", x => x.MaID);
                    table.ForeignKey(
                        name: "FK_Chuyende_Tailieu_Chuyende_ChuyendeID",
                        column: x => x.ChuyendeID,
                        principalTable: "Chuyende",
                        principalColumn: "MaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chuyende_Dapan",
                columns: table => new
                {
                    MaID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    ChuyendeID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Chuyende_CauhoiID = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    Ten = table.Column<string>(type: "NVARCHAR(200)", nullable: false),
                    Dung = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chuyende_Dapan", x => x.MaID);
                    table.ForeignKey(
                        name: "FK_Chuyende_Dapan_Chuyende_Cauhoi_Chuyende_CauhoiID",
                        column: x => x.Chuyende_CauhoiID,
                        principalTable: "Chuyende_Cauhoi",
                        principalColumn: "MaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyende_Cauhoi_ChuyendeID",
                table: "Chuyende_Cauhoi",
                column: "ChuyendeID");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyende_Dapan_Chuyende_CauhoiID",
                table: "Chuyende_Dapan",
                column: "Chuyende_CauhoiID");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyende_Giangvien_ChuyendeID",
                table: "Chuyende_Giangvien",
                column: "ChuyendeID");

            migrationBuilder.CreateIndex(
                name: "IX_Chuyende_Tailieu_ChuyendeID",
                table: "Chuyende_Tailieu",
                column: "ChuyendeID");

            migrationBuilder.CreateIndex(
                name: "IX_Giangvien_UserId",
                table: "Giangvien",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Hocvien_UserId",
                table: "Hocvien",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Chungchi");

            migrationBuilder.DropTable(
                name: "Chungchi_Hocvien");

            migrationBuilder.DropTable(
                name: "Chuyende_Dapan");

            migrationBuilder.DropTable(
                name: "Chuyende_Giangvien");

            migrationBuilder.DropTable(
                name: "Chuyende_Tailieu");

            migrationBuilder.DropTable(
                name: "Giangvien");

            migrationBuilder.DropTable(
                name: "Hocvien");

            migrationBuilder.DropTable(
                name: "Khoahoc");

            migrationBuilder.DropTable(
                name: "Khoahoc_Chuyende");

            migrationBuilder.DropTable(
                name: "Khoahoc_DmTrangthai");

            migrationBuilder.DropTable(
                name: "Khoahoc_Giangvien");

            migrationBuilder.DropTable(
                name: "Khoahoc_Hocvien");

            migrationBuilder.DropTable(
                name: "Lichhoc");

            migrationBuilder.DropTable(
                name: "Lichhoc_Giangvien_Diemdanh");

            migrationBuilder.DropTable(
                name: "Lichhoc_Hocvien_Diemdanh");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Chuyende_Cauhoi");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Chuyende");
        }
    }
}
