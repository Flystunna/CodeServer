﻿// <auto-generated />
using System;
using CodeServer.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeServer.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodeServer.Core.Models.project", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("external_id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("last_modified_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("sdlc_systemid")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("sdlc_systemid");

                    b.HasIndex("external_id", "sdlc_systemid")
                        .IsUnique()
                        .HasFilter("[external_id] IS NOT NULL");

                    b.ToTable("project");
                });

            modelBuilder.Entity("CodeServer.Core.Models.sdlc_system", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("base_url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("created_date")
                        .HasColumnType("datetime2");

                    b.Property<string>("description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("last_modified_date")
                        .HasColumnType("datetime2");

                    b.HasKey("id");

                    b.ToTable("sdlc_system");
                });

            modelBuilder.Entity("CodeServer.Core.Models.project", b =>
                {
                    b.HasOne("CodeServer.Core.Models.sdlc_system", "sdlc_system")
                        .WithMany()
                        .HasForeignKey("sdlc_systemid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
