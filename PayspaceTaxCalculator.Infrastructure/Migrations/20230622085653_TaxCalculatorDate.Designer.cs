﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PayspaceTaxCalculator.Infrastructure;

#nullable disable

namespace PayspaceTaxCalculator.Infrastructure.Migrations
{
    [DbContext(typeof(PayspaceDbContext))]
    [Migration("20230622085653_TaxCalculatorDate")]
    partial class TaxCalculatorDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PayspaceTaxCalculator.Domain.PostalCodeTaxCalculationType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<int>("TaxCalculationType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("PostalCodeTaxCalculationTypes");
                });

            modelBuilder.Entity("PayspaceTaxCalculator.Domain.ProgressiveTaxRate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("From")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("To")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("ProgressiveTaxRates");
                });

            modelBuilder.Entity("PayspaceTaxCalculator.Domain.TaxCalculator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("AnnualIncome")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CalculatedTaxAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TaxCalculators");
                });
#pragma warning restore 612, 618
        }
    }
}
