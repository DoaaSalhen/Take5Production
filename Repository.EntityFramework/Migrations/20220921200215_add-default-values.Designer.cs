﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Repository.EntityFramework;

namespace Repository.EntityFramework.Migrations
{
    [DbContext(typeof(APPDBContext))]
    [Migration("20220921200215_add-default-values")]
    partial class adddefaultvalues
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Take5.Models.Models.MasterModels.Danger", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("DangerCategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("step")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DangerCategoryId");

                    b.ToTable("Dangers");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.DangerCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DangerCategories");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.Driver", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Drivers");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.Instruction", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<int>("Step")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Instructions");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.Question", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<int>("Step")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.Trip", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<long>("DriverId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<long>("TruckId")
                        .HasColumnType("bigint");

                    b.Property<string>("TruckId1")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DriverId");

                    b.HasIndex("TruckId1");

                    b.ToTable("Trips");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.TripDanger", b =>
                {
                    b.Property<long>("TripId")
                        .HasColumnType("bigint");

                    b.Property<int>("DangerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("TripId", "DangerId");

                    b.HasIndex("DangerId");

                    b.ToTable("TripDangers");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.TripDangerControl", b =>
                {
                    b.Property<long>("TripId")
                        .HasColumnType("bigint");

                    b.Property<int>("DangerId")
                        .HasColumnType("int");

                    b.Property<string>("ControlMeasure")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TripId", "DangerId");

                    b.HasIndex("DangerId");

                    b.ToTable("TripDangerControls");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.TripQuestion", b =>
                {
                    b.Property<long>("TripId")
                        .HasColumnType("bigint");

                    b.Property<int>("QuestionId")
                        .HasColumnType("int");

                    b.Property<bool>("Answer")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("TripId", "QuestionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("TripQuestions");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.Truck", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDelted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Trucks");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.Danger", b =>
                {
                    b.HasOne("Take5.Models.Models.MasterModels.DangerCategory", "DangerCategory")
                        .WithMany()
                        .HasForeignKey("DangerCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DangerCategory");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.Trip", b =>
                {
                    b.HasOne("Take5.Models.Models.MasterModels.Driver", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Take5.Models.Models.MasterModels.Truck", "Truck")
                        .WithMany()
                        .HasForeignKey("TruckId1");

                    b.Navigation("Driver");

                    b.Navigation("Truck");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.TripDanger", b =>
                {
                    b.HasOne("Take5.Models.Models.MasterModels.Danger", "Danger")
                        .WithMany()
                        .HasForeignKey("DangerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Take5.Models.Models.MasterModels.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Danger");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.TripDangerControl", b =>
                {
                    b.HasOne("Take5.Models.Models.MasterModels.Danger", "Danger")
                        .WithMany()
                        .HasForeignKey("DangerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Take5.Models.Models.MasterModels.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Danger");

                    b.Navigation("Trip");
                });

            modelBuilder.Entity("Take5.Models.Models.MasterModels.TripQuestion", b =>
                {
                    b.HasOne("Take5.Models.Models.MasterModels.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Take5.Models.Models.MasterModels.Trip", "Trip")
                        .WithMany()
                        .HasForeignKey("TripId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("Trip");
                });
#pragma warning restore 612, 618
        }
    }
}
