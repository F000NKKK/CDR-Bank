﻿// <auto-generated />
using System;
using CDR_Bank.DataAccess.Banking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CDR_Bank.DataAccess.Migrations
{
    [DbContext(typeof(BankingDataContext))]
    [Migration("20250503180904_fix")]
    partial class fix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("CDR_Bank.DataAccess.Banking.Entities.AccountTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("BankingAccountId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CounterpartyAccountId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("BankingAccountId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("CDR_Bank.DataAccess.Banking.Entities.BankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("CreditLimit")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("IsMain")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("TelephoneNumber")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AccountNumber")
                        .IsUnique();

                    b.HasIndex("IsMain");

                    b.HasIndex("TelephoneNumber");

                    b.HasIndex("UserId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("CDR_Bank.DataAccess.Banking.Entities.AccountTransaction", b =>
                {
                    b.HasOne("CDR_Bank.DataAccess.Banking.Entities.BankAccount", "BankingAccount")
                        .WithMany("Transactions")
                        .HasForeignKey("BankingAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BankingAccount");
                });

            modelBuilder.Entity("CDR_Bank.DataAccess.Banking.Entities.BankAccount", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
