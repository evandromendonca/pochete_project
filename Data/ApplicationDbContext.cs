using Microsoft.EntityFrameworkCore;
using Pochete.Models;
using Pochete.Models.Wallets;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Pochete.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRate> CurrenciesRates { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<SnapshotSecurity> SnapshotSecurity { get; set; }
        public DbSet<SnapshotCash> SnapshotCash { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            // CurrencyRate primary key
            builder.Entity<CurrencyRate>().HasKey(o => new {o.CurrencyId, o.ReferenceCurrencyId, o.Date});

            // CurrencyRate foreign key with Currency
            builder.Entity<CurrencyRate>()
                .HasOne(m => m.Currency)
                .WithMany(t => t.CurrencyRates)
                .HasForeignKey(m => m.CurrencyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // CurrencyRate foreign key with Currency for CurrencyReference
            builder.Entity<CurrencyRate>()
                .HasOne(m => m.ReferenceCurrency)
                .WithMany(t => t.CurrencyReferences)
                .HasForeignKey(m => m.ReferenceCurrencyId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // // SnapshotSecurity foreign key with Wallet
            // builder.Entity<SnapshotSecurity>()
            //     .HasOne(m => m.Wallet)
            //     .WithMany(t => t.SnapshotsSecuritys)
            //     .HasForeignKey(m => m.WalletId)
            //     .OnDelete(DeleteBehavior.Cascade);
        }
    }
}