﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicTool.DataAccess.Data;

#nullable disable

namespace MusicTool.DataAccess.Migrations
{
    [DbContext(typeof(MusicToolDbContext))]
    [Migration("20240603171224_add_HSDvip")]
    partial class add_HSDvip
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MusicTool.Models.Domain.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("Varchar(100)");

                    b.Property<string>("Name")
                        .HasColumnType("Nvarchar(100)");

                    b.Property<string>("Password")
                        .HasColumnType("Varchar(60)");

                    b.Property<string>("PhotoName")
                        .HasColumnType("Varchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<bool>("Type")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("Varchar(50)");

                    b.Property<string>("UserType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("VipExpires")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("Nvarchar(100)");

                    b.Property<string>("NameNoDiacritics")
                        .HasColumnType("Varchar(100)");

                    b.Property<string>("PhotoName")
                        .HasColumnType("Varchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Singer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("LoadingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("Nvarchar(50)");

                    b.Property<string>("NameNoDiacritics")
                        .HasColumnType("Varchar(50)");

                    b.Property<string>("PhotoName")
                        .HasColumnType("Varchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Singer");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LoadingDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Lyrics")
                        .HasColumnType("Nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("Nvarchar(max)");

                    b.Property<string>("NameNoDiacritics")
                        .HasColumnType("Varchar(max)");

                    b.Property<int?>("NumberOfDownloads")
                        .HasColumnType("int");

                    b.Property<int?>("NumberOfListens")
                        .HasColumnType("int");

                    b.Property<string>("PhotoName")
                        .HasColumnType("Varchar(max)");

                    b.Property<string>("SongFile")
                        .HasColumnType("Varchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Song");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Song_Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int?>("SongId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("SongId");

                    b.ToTable("Song_Category");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Song_Singer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("SingerId")
                        .HasColumnType("int");

                    b.Property<int?>("SongId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SingerId");

                    b.HasIndex("SongId");

                    b.ToTable("Song_Singer");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Song", b =>
                {
                    b.HasOne("MusicTool.Models.Domain.Account", "Account")
                        .WithMany("Songs")
                        .HasForeignKey("AccountId");

                    b.Navigation("Account");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Song_Category", b =>
                {
                    b.HasOne("MusicTool.Models.Domain.Category", "Category")
                        .WithMany("Song_Category")
                        .HasForeignKey("CategoryId");

                    b.HasOne("MusicTool.Models.Domain.Song", "Song")
                        .WithMany("Song_Category")
                        .HasForeignKey("SongId");

                    b.Navigation("Category");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Song_Singer", b =>
                {
                    b.HasOne("MusicTool.Models.Domain.Singer", "Singer")
                        .WithMany("Song_Singer")
                        .HasForeignKey("SingerId");

                    b.HasOne("MusicTool.Models.Domain.Song", "Song")
                        .WithMany("Song_Singer")
                        .HasForeignKey("SongId");

                    b.Navigation("Singer");

                    b.Navigation("Song");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Account", b =>
                {
                    b.Navigation("Songs");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Category", b =>
                {
                    b.Navigation("Song_Category");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Singer", b =>
                {
                    b.Navigation("Song_Singer");
                });

            modelBuilder.Entity("MusicTool.Models.Domain.Song", b =>
                {
                    b.Navigation("Song_Category");

                    b.Navigation("Song_Singer");
                });
#pragma warning restore 612, 618
        }
    }
}
