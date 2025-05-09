﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(AppCrimeMapContext))]
    [Migration("20250114014741_AddGeoPointColumnToCrime6")]
    partial class AddGeoPointColumnToCrime6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "postgis");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Crime", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Applicant")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CrimeDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("LawsuitId")
                        .HasColumnType("uuid");

                    b.Property<string>("Location")
                        .HasColumnType("text");

                    b.Property<Point>("Point")
                        .HasColumnType("geometry");

                    b.Property<Guid>("TypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("WantedPersonId")
                        .HasColumnType("uuid");

                    b.ComplexProperty<Dictionary<string, object>>("PointMe", "Domain.Entities.Crime.PointMe#PointMe", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<decimal>("Latitude")
                                .HasColumnType("numeric");

                            b1.Property<decimal>("Longitude")
                                .HasColumnType("numeric");
                        });

                    b.HasKey("Id");

                    b.HasIndex("LawsuitId");

                    b.HasIndex("TypeId");

                    b.HasIndex("WantedPersonId");

                    b.ToTable("Crimes");
                });

            modelBuilder.Entity("Domain.Entities.CrimeType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Color")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Link")
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CrimeTypes");
                });

            modelBuilder.Entity("Domain.Entities.Lawsuit", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Decision")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DecisionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Judge")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("JudicialActs")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("PersonId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ReceiptDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Lawsuits");
                });

            modelBuilder.Entity("Domain.Entities.WantedPerson", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AddInfo")
                        .HasColumnType("text");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Patronymic")
                        .HasColumnType("text");

                    b.Property<string>("RegistrationAddress")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("WantedPersons");
                });

            modelBuilder.Entity("Domain.Entities.Crime", b =>
                {
                    b.HasOne("Domain.Entities.Lawsuit", "Lawsuit")
                        .WithMany()
                        .HasForeignKey("LawsuitId");

                    b.HasOne("Domain.Entities.CrimeType", "Type")
                        .WithMany("Crimes")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.WantedPerson", "WantedPerson")
                        .WithMany("Crimes")
                        .HasForeignKey("WantedPersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lawsuit");

                    b.Navigation("Type");

                    b.Navigation("WantedPerson");
                });

            modelBuilder.Entity("Domain.Entities.Lawsuit", b =>
                {
                    b.HasOne("Domain.Entities.WantedPerson", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Domain.Entities.CrimeType", b =>
                {
                    b.Navigation("Crimes");
                });

            modelBuilder.Entity("Domain.Entities.WantedPerson", b =>
                {
                    b.Navigation("Crimes");
                });
#pragma warning restore 612, 618
        }
    }
}
