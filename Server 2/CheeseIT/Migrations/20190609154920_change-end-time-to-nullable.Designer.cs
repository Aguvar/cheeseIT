﻿// <auto-generated />
using System;
using CheeseIT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CheeseIT.Migrations
{
    [DbContext(typeof(CheeseContext))]
    [Migration("20190609154920_change-end-time-to-nullable")]
    partial class changeendtimetonullable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CheeseIT.Models.Cheese", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Base64Image");

                    b.Property<int>("DaysToRipe");

                    b.Property<string>("Description");

                    b.Property<float>("HumidityThreshold");

                    b.Property<float>("IdealHumidity");

                    b.Property<float>("IdealTemperature");

                    b.Property<string>("Name");

                    b.Property<float>("TemperatureThreshold");

                    b.HasKey("Id");

                    b.ToTable("Cheeses");
                });

            modelBuilder.Entity("CheeseIT.Models.Experiment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndTime");

                    b.Property<DateTime>("EstimatedEndTime");

                    b.Property<float>("HumidityThreshold");

                    b.Property<float>("IdealHumidity");

                    b.Property<float>("IdealTemperature");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.Property<float>("TemperatureThreshold");

                    b.HasKey("Id");

                    b.ToTable("Experiments");
                });

            modelBuilder.Entity("CheeseIT.Models.Measurement", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTime");

                    b.Property<Guid?>("ExperimentId");

                    b.Property<float>("Humidity");

                    b.Property<Guid?>("RipeningId");

                    b.Property<float>("Temperature");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentId");

                    b.HasIndex("RipeningId");

                    b.ToTable("Measurement");
                });

            modelBuilder.Entity("CheeseIT.Models.Observation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Base64Image");

                    b.Property<DateTime>("DateTime");

                    b.Property<Guid?>("ExperimentId");

                    b.Property<string>("Note");

                    b.HasKey("Id");

                    b.HasIndex("ExperimentId");

                    b.ToTable("Observation");
                });

            modelBuilder.Entity("CheeseIT.Models.Ripening", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("CheeseId");

                    b.Property<DateTime?>("EndTime");

                    b.Property<DateTime>("StartDate");

                    b.HasKey("Id");

                    b.HasIndex("CheeseId");

                    b.ToTable("Ripenings");
                });

            modelBuilder.Entity("CheeseIT.Models.Measurement", b =>
                {
                    b.HasOne("CheeseIT.Models.Experiment")
                        .WithMany("Measurements")
                        .HasForeignKey("ExperimentId");

                    b.HasOne("CheeseIT.Models.Ripening")
                        .WithMany("Measurements")
                        .HasForeignKey("RipeningId");
                });

            modelBuilder.Entity("CheeseIT.Models.Observation", b =>
                {
                    b.HasOne("CheeseIT.Models.Experiment")
                        .WithMany("Observations")
                        .HasForeignKey("ExperimentId");
                });

            modelBuilder.Entity("CheeseIT.Models.Ripening", b =>
                {
                    b.HasOne("CheeseIT.Models.Cheese", "Cheese")
                        .WithMany()
                        .HasForeignKey("CheeseId");
                });
#pragma warning restore 612, 618
        }
    }
}
