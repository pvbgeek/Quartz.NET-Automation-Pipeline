﻿// <auto-generated />
using System;
using JobScheduler.Processing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JobScheduler.Processing.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("JobScheduler.Processing.AggregatedSales", b =>
                {
                    b.Property<string>("Region")
                        .HasColumnType("text");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<decimal>("AverageSales")
                        .HasColumnType("numeric");

                    b.Property<string>("TopProduct")
                        .HasColumnType("text");

                    b.Property<decimal>("TotalSales")
                        .HasColumnType("numeric");

                    b.HasKey("Region", "Category");

                    b.ToTable("AggregatedSales");
                });

            modelBuilder.Entity("JobScheduler.Processing.JobMetadata", b =>
                {
                    b.Property<string>("JobName")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastRunTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("JobName");

                    b.ToTable("JobMetadata");
                });

            modelBuilder.Entity("JobScheduler.Processing.MonthlyTrends", b =>
                {
                    b.Property<DateTime>("Month")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<decimal>("SalesGrowth")
                        .HasColumnType("numeric");

                    b.HasKey("Month", "Category");

                    b.ToTable("MonthlyTrends");
                });

            modelBuilder.Entity("JobScheduler.Processing.StagingData", b =>
                {
                    b.Property<long>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("TransactionID"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<string>("Category")
                        .HasColumnType("text");

                    b.Property<int>("CustomerID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ProductID")
                        .HasColumnType("integer");

                    b.Property<string>("Region")
                        .HasColumnType("text");

                    b.HasKey("TransactionID");

                    b.ToTable("StagingData");
                });
#pragma warning restore 612, 618
        }
    }
}
