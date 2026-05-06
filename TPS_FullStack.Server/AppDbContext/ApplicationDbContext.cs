using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TPS_FullStack.Server.Entities;

namespace TPS_FullStack.Server.AppDbContext
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Giangvien> Giangvien { get; set; }
        public DbSet<Hocvien> Hocvien { get; set; }
        public DbSet<Chuyende> Chuyende { get; set; }
        public DbSet<Chuyende_Tailieu> Chuyende_Tailieu { get; set; }
        public DbSet<Chuyende_Cauhoi> Chuyende_Cauhoi { get; set; }
        public DbSet<Chuyende_Dapan> Chuyende_Dapan { get; set; }
        public DbSet<Khoahoc> Khoahoc { get; set; }
        public DbSet<Khoahoc_Hocvien> Khoahoc_Hocvien { get; set; }
        public DbSet<Khoahoc_Giangvien> Khoahoc_Giangvien { get; set; }
        public DbSet<Khoahoc_Chuyende> Khoahoc_Chuyende { get; set; }
        public DbSet<Khoahoc_dmTrangthai> Khoahoc_DmTrangthai { get; set; }
        public DbSet<Chungchi> Chungchi { get; set; }
        public DbSet<Chungchi_Hocvien> Chungchi_Hocvien { get; set; }
        public DbSet<Lichhoc> Lichhoc { get; set; }
        public DbSet<Lichhoc_Ct> Lichhoc_Ct { get; set; }
        public DbSet<Lichhoc_Ct_Diemdanh> Lichhoc_Ct_Diemdanh { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);
                entity.HasOne(rt => rt.User)
                        .WithMany(u => u.RefreshTokens)
                        .HasForeignKey(rt => rt.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}


