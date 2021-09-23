﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NiN.Database;

namespace NiN.Database.Migrations
{
    [DbContext(typeof(NiNContext))]
    partial class NiNContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NiN.Database.Models.Basistrinn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Navn")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("TrinnId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TrinnId");

                    b.ToTable("Basistrinn");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.Kode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definisjon")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Kategori")
                        .HasColumnType("int");

                    b.Property<string>("KodeName")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Kode");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Kode");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.LKMKode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Kode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LkmKategori")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LKMKode");
                });

            modelBuilder.Entity("NiN.Database.Models.Grunntype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HovedtypeId")
                        .HasColumnType("int");

                    b.Property<string>("Navn")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("HovedtypeId");

                    b.ToTable("Grunntype");
                });

            modelBuilder.Entity("NiN.Database.Models.Hovedtype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HovedtypegruppeId")
                        .HasColumnType("int");

                    b.Property<string>("Navn")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("HovedtypegruppeId");

                    b.ToTable("Hovedtype");
                });

            modelBuilder.Entity("NiN.Database.Models.Hovedtypegruppe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("NatursystemId")
                        .HasColumnType("int");

                    b.Property<string>("Navn")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("NatursystemId");

                    b.ToTable("Hovedtypegruppe");
                });

            modelBuilder.Entity("NiN.Database.Models.Kartleggingsenhet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definisjon")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("HovedtypeId")
                        .HasColumnType("int");

                    b.Property<string>("KodeId")
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("Malestokk")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("HovedtypeId");

                    b.ToTable("Kartleggingsenhet");
                });

            modelBuilder.Entity("NiN.Database.Models.Miljovariabel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("HovedtypeId")
                        .HasColumnType("int");

                    b.Property<int?>("KodeId")
                        .HasColumnType("int");

                    b.Property<string>("Navn")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("HovedtypeId");

                    b.HasIndex("KodeId");

                    b.ToTable("Miljovariabel");
                });

            modelBuilder.Entity("NiN.Database.Models.Natursystem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Navn")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Natursystem");
                });

            modelBuilder.Entity("NiN.Database.Models.Trinn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("MiljovariabelId")
                        .HasColumnType("int");

                    b.Property<string>("Navn")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("MiljovariabelId");

                    b.ToTable("Trinn");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.BasistrinnKode", b =>
                {
                    b.HasBaseType("NiN.Database.Models.Codes.Kode");

                    b.Property<int>("BasistrinnId")
                        .HasColumnType("int");

                    b.HasIndex("BasistrinnId")
                        .IsUnique()
                        .HasFilter("[BasistrinnId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("BasistrinnKode");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.GrunntypeKode", b =>
                {
                    b.HasBaseType("NiN.Database.Models.Codes.Kode");

                    b.Property<int>("GrunntypeId")
                        .HasColumnType("int");

                    b.HasIndex("GrunntypeId")
                        .IsUnique()
                        .HasFilter("[GrunntypeId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("GrunntypeKode");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.HovedtypeKode", b =>
                {
                    b.HasBaseType("NiN.Database.Models.Codes.Kode");

                    b.Property<int>("HovedtypeId")
                        .HasColumnType("int");

                    b.HasIndex("HovedtypeId")
                        .IsUnique()
                        .HasFilter("[HovedtypeId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("HovedtypeKode");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.HovedtypegruppeKode", b =>
                {
                    b.HasBaseType("NiN.Database.Models.Codes.Kode");

                    b.Property<int>("HovedtypegruppeId")
                        .HasColumnType("int");

                    b.HasIndex("HovedtypegruppeId")
                        .IsUnique()
                        .HasFilter("[HovedtypegruppeId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("HovedtypegruppeKode");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.NatursystemKode", b =>
                {
                    b.HasBaseType("NiN.Database.Models.Codes.Kode");

                    b.Property<int>("NatursystemId")
                        .HasColumnType("int");

                    b.HasIndex("NatursystemId")
                        .IsUnique()
                        .HasFilter("[NatursystemId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("NatursystemKode");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.TrinnKode", b =>
                {
                    b.HasBaseType("NiN.Database.Models.Codes.Kode");

                    b.Property<int>("TrinnId")
                        .HasColumnType("int");

                    b.HasIndex("TrinnId")
                        .IsUnique()
                        .HasFilter("[TrinnId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("TrinnKode");
                });

            modelBuilder.Entity("NiN.Database.Models.Basistrinn", b =>
                {
                    b.HasOne("NiN.Database.Models.Trinn", "Trinn")
                        .WithMany("Basistrinn")
                        .HasForeignKey("TrinnId");

                    b.Navigation("Trinn");
                });

            modelBuilder.Entity("NiN.Database.Models.Grunntype", b =>
                {
                    b.HasOne("NiN.Database.Models.Hovedtype", "Hovedtype")
                        .WithMany("UnderordnetKoder")
                        .HasForeignKey("HovedtypeId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Hovedtype");
                });

            modelBuilder.Entity("NiN.Database.Models.Hovedtype", b =>
                {
                    b.HasOne("NiN.Database.Models.Hovedtypegruppe", "Hovedtypegruppe")
                        .WithMany("UnderordnetKoder")
                        .HasForeignKey("HovedtypegruppeId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Hovedtypegruppe");
                });

            modelBuilder.Entity("NiN.Database.Models.Hovedtypegruppe", b =>
                {
                    b.HasOne("NiN.Database.Models.Natursystem", "Natursystem")
                        .WithMany("UnderordnetKoder")
                        .HasForeignKey("NatursystemId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Natursystem");
                });

            modelBuilder.Entity("NiN.Database.Models.Kartleggingsenhet", b =>
                {
                    b.HasOne("NiN.Database.Models.Hovedtype", "Hovedtype")
                        .WithMany("Kartleggingsenheter")
                        .HasForeignKey("HovedtypeId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("Hovedtype");
                });

            modelBuilder.Entity("NiN.Database.Models.Miljovariabel", b =>
                {
                    b.HasOne("NiN.Database.Models.Hovedtype", "Hovedtype")
                        .WithMany("Miljovariabler")
                        .HasForeignKey("HovedtypeId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("NiN.Database.Models.Codes.LKMKode", "Kode")
                        .WithMany()
                        .HasForeignKey("KodeId");

                    b.Navigation("Hovedtype");

                    b.Navigation("Kode");
                });

            modelBuilder.Entity("NiN.Database.Models.Trinn", b =>
                {
                    b.HasOne("NiN.Database.Models.Miljovariabel", "Miljovariabel")
                        .WithMany("Trinn")
                        .HasForeignKey("MiljovariabelId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Miljovariabel");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.BasistrinnKode", b =>
                {
                    b.HasOne("NiN.Database.Models.Basistrinn", "Basistrinn")
                        .WithOne("Kode")
                        .HasForeignKey("NiN.Database.Models.Codes.BasistrinnKode", "BasistrinnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Basistrinn");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.GrunntypeKode", b =>
                {
                    b.HasOne("NiN.Database.Models.Grunntype", "Grunntype")
                        .WithOne("Kode")
                        .HasForeignKey("NiN.Database.Models.Codes.GrunntypeKode", "GrunntypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grunntype");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.HovedtypeKode", b =>
                {
                    b.HasOne("NiN.Database.Models.Hovedtype", "Hovedtype")
                        .WithOne("Kode")
                        .HasForeignKey("NiN.Database.Models.Codes.HovedtypeKode", "HovedtypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hovedtype");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.HovedtypegruppeKode", b =>
                {
                    b.HasOne("NiN.Database.Models.Hovedtypegruppe", "Hovedtypegruppe")
                        .WithOne("Kode")
                        .HasForeignKey("NiN.Database.Models.Codes.HovedtypegruppeKode", "HovedtypegruppeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Hovedtypegruppe");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.NatursystemKode", b =>
                {
                    b.HasOne("NiN.Database.Models.Natursystem", "Natursystem")
                        .WithOne("Kode")
                        .HasForeignKey("NiN.Database.Models.Codes.NatursystemKode", "NatursystemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Natursystem");
                });

            modelBuilder.Entity("NiN.Database.Models.Codes.TrinnKode", b =>
                {
                    b.HasOne("NiN.Database.Models.Trinn", "Trinn")
                        .WithOne("Kode")
                        .HasForeignKey("NiN.Database.Models.Codes.TrinnKode", "TrinnId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trinn");
                });

            modelBuilder.Entity("NiN.Database.Models.Basistrinn", b =>
                {
                    b.Navigation("Kode");
                });

            modelBuilder.Entity("NiN.Database.Models.Grunntype", b =>
                {
                    b.Navigation("Kode");
                });

            modelBuilder.Entity("NiN.Database.Models.Hovedtype", b =>
                {
                    b.Navigation("Kartleggingsenheter");

                    b.Navigation("Kode");

                    b.Navigation("Miljovariabler");

                    b.Navigation("UnderordnetKoder");
                });

            modelBuilder.Entity("NiN.Database.Models.Hovedtypegruppe", b =>
                {
                    b.Navigation("Kode")
                        .IsRequired();

                    b.Navigation("UnderordnetKoder");
                });

            modelBuilder.Entity("NiN.Database.Models.Miljovariabel", b =>
                {
                    b.Navigation("Trinn");
                });

            modelBuilder.Entity("NiN.Database.Models.Natursystem", b =>
                {
                    b.Navigation("Kode")
                        .IsRequired();

                    b.Navigation("UnderordnetKoder");
                });

            modelBuilder.Entity("NiN.Database.Models.Trinn", b =>
                {
                    b.Navigation("Basistrinn");

                    b.Navigation("Kode");
                });
#pragma warning restore 612, 618
        }
    }
}
