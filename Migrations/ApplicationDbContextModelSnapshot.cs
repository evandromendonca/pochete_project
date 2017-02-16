using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Pochete.Data;

namespace Pochete.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("Pochete.Models.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("Pochete.Models.CurrencyRate", b =>
                {
                    b.Property<int>("CurrencyId");

                    b.Property<int>("ReferenceCurrencyId");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("Rate");

                    b.HasKey("CurrencyId", "ReferenceCurrencyId", "Date");

                    b.HasIndex("ReferenceCurrencyId");

                    b.ToTable("CurrenciesRates");
                });

            modelBuilder.Entity("Pochete.Models.Wallets.SnapshotSecurity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<int>("WalletId");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("SnapshotSecurity");

                    b.HasDiscriminator<string>("Discriminator").HasValue("SnapshotSecurity");
                });

            modelBuilder.Entity("Pochete.Models.Wallets.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("Pochete.Models.Wallets.SnapshotCash", b =>
                {
                    b.HasBaseType("Pochete.Models.Wallets.SnapshotSecurity");

                    b.Property<int>("CurrencyId");

                    b.Property<decimal>("Value");

                    b.HasIndex("CurrencyId");

                    b.ToTable("SnapshotCash");

                    b.HasDiscriminator().HasValue("SnapshotCash");
                });

            modelBuilder.Entity("Pochete.Models.CurrencyRate", b =>
                {
                    b.HasOne("Pochete.Models.Currency", "Currency")
                        .WithMany("CurrencyRates")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Pochete.Models.Currency", "ReferenceCurrency")
                        .WithMany("CurrencyReferences")
                        .HasForeignKey("ReferenceCurrencyId");
                });

            modelBuilder.Entity("Pochete.Models.Wallets.SnapshotSecurity", b =>
                {
                    b.HasOne("Pochete.Models.Wallets.Wallet", "Wallet")
                        .WithMany("SnapshotsSecuritys")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Pochete.Models.Wallets.SnapshotCash", b =>
                {
                    b.HasOne("Pochete.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
