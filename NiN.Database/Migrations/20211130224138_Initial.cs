﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NiN.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NinVersion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NinVersion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Basistrinn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basistrinn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Basistrinn_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LKMKode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    LkmKategori = table.Column<int>(type: "int", nullable: false),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKMKode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LKMKode_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Natursystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natursystem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Natursystem_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VarietyLevel0",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarietyLevel0", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarietyLevel0_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hovedtypegruppe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NatursystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hovedtypegruppe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hovedtypegruppe_Natursystem_NatursystemId",
                        column: x => x.NatursystemId,
                        principalTable: "Natursystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hovedtypegruppe_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VarietyLevel1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverordnetKodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarietyLevel1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarietyLevel1_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VarietyLevel1_VarietyLevel0_OverordnetKodeId",
                        column: x => x.OverordnetKodeId,
                        principalTable: "VarietyLevel0",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hovedtype",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HovedtypegruppeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hovedtype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hovedtype_Hovedtypegruppe_HovedtypegruppeId",
                        column: x => x.HovedtypegruppeId,
                        principalTable: "Hovedtypegruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Hovedtype_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VarietyLevel2",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverordnetKodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarietyLevel2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarietyLevel2_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VarietyLevel2_VarietyLevel1_OverordnetKodeId",
                        column: x => x.OverordnetKodeId,
                        principalTable: "VarietyLevel1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grunntype",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HovedtypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grunntype", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grunntype_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grunntype_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kartleggingsenhet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Malestokk = table.Column<int>(type: "int", nullable: false),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HovedtypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kartleggingsenhet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kartleggingsenhet_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kartleggingsenhet_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Miljovariabel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HovedtypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Miljovariabel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Miljovariabel_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Miljovariabel_LKMKode_KodeId",
                        column: x => x.KodeId,
                        principalTable: "LKMKode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Miljovariabel_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VarietyLevel3",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverordnetKodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarietyLevel3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarietyLevel3_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VarietyLevel3_VarietyLevel2_OverordnetKodeId",
                        column: x => x.OverordnetKodeId,
                        principalTable: "VarietyLevel2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BasistrinnGrunntype",
                columns: table => new
                {
                    BasistrinnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrunntypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasistrinnGrunntype", x => new { x.BasistrinnId, x.GrunntypeId });
                    table.ForeignKey(
                        name: "FK_BasistrinnGrunntype_Basistrinn_BasistrinnId",
                        column: x => x.BasistrinnId,
                        principalTable: "Basistrinn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasistrinnGrunntype_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrunntypeKartleggingsenhet",
                columns: table => new
                {
                    GrunntypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KartleggingsenhetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrunntypeKartleggingsenhet", x => new { x.GrunntypeId, x.KartleggingsenhetId });
                    table.ForeignKey(
                        name: "FK_GrunntypeKartleggingsenhet_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrunntypeKartleggingsenhet_Kartleggingsenhet_KartleggingsenhetId",
                        column: x => x.KartleggingsenhetId,
                        principalTable: "Kartleggingsenhet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrunntypeMiljovariabel",
                columns: table => new
                {
                    GrunntypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MiljovariabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrunntypeMiljovariabel", x => new { x.GrunntypeId, x.MiljovariabelId });
                    table.ForeignKey(
                        name: "FK_GrunntypeMiljovariabel_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrunntypeMiljovariabel_Miljovariabel_MiljovariabelId",
                        column: x => x.MiljovariabelId,
                        principalTable: "Miljovariabel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trinn",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MiljovariabelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trinn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trinn_Miljovariabel_MiljovariabelId",
                        column: x => x.MiljovariabelId,
                        principalTable: "Miljovariabel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Trinn_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VarietyLevel4",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverordnetKodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarietyLevel4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarietyLevel4_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VarietyLevel4_VarietyLevel3_OverordnetKodeId",
                        column: x => x.OverordnetKodeId,
                        principalTable: "VarietyLevel3",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BasistrinnTrinn",
                columns: table => new
                {
                    BasistrinnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrinnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasistrinnTrinn", x => new { x.BasistrinnId, x.TrinnId });
                    table.ForeignKey(
                        name: "FK_BasistrinnTrinn_Basistrinn_BasistrinnId",
                        column: x => x.BasistrinnId,
                        principalTable: "Basistrinn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasistrinnTrinn_Trinn_TrinnId",
                        column: x => x.TrinnId,
                        principalTable: "Trinn",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Kode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KodeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kategori = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrunntypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HovedtypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HovedtypegruppeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    KartleggingsenhetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NatursystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrinnId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kode_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Hovedtype_HovedtypeId",
                        column: x => x.HovedtypeId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Hovedtypegruppe_HovedtypegruppeId",
                        column: x => x.HovedtypegruppeId,
                        principalTable: "Hovedtypegruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Kartleggingsenhet_KartleggingsenhetId",
                        column: x => x.KartleggingsenhetId,
                        principalTable: "Kartleggingsenhet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_Natursystem_NatursystemId",
                        column: x => x.NatursystemId,
                        principalTable: "Natursystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Kode_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kode_Trinn_TrinnId",
                        column: x => x.TrinnId,
                        principalTable: "Trinn",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VarietyLevel5",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverordnetKodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarietyLevel5", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarietyLevel5_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VarietyLevel5_VarietyLevel4_OverordnetKodeId",
                        column: x => x.OverordnetKodeId,
                        principalTable: "VarietyLevel4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VariasjonKode",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KodeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    VarietyCategory = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VarietyLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VarietyLevel1Code_VarietyLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VarietyLevel2Code_VarietyLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VarietyLevel3Code_VarietyLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VarietyLevel4Code_VarietyLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VarietyLevel5Code_VarietyLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VersionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariasjonKode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariasjonKode_NinVersion_VersionId",
                        column: x => x.VersionId,
                        principalTable: "NinVersion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VariasjonKode_VarietyLevel0_VarietyLevelId",
                        column: x => x.VarietyLevelId,
                        principalTable: "VarietyLevel0",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VariasjonKode_VarietyLevel1_VarietyLevel1Code_VarietyLevelId",
                        column: x => x.VarietyLevel1Code_VarietyLevelId,
                        principalTable: "VarietyLevel1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VariasjonKode_VarietyLevel2_VarietyLevel2Code_VarietyLevelId",
                        column: x => x.VarietyLevel2Code_VarietyLevelId,
                        principalTable: "VarietyLevel2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VariasjonKode_VarietyLevel3_VarietyLevel3Code_VarietyLevelId",
                        column: x => x.VarietyLevel3Code_VarietyLevelId,
                        principalTable: "VarietyLevel3",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VariasjonKode_VarietyLevel4_VarietyLevel4Code_VarietyLevelId",
                        column: x => x.VarietyLevel4Code_VarietyLevelId,
                        principalTable: "VarietyLevel4",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VariasjonKode_VarietyLevel5_VarietyLevel5Code_VarietyLevelId",
                        column: x => x.VarietyLevel5Code_VarietyLevelId,
                        principalTable: "VarietyLevel5",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Basistrinn_VersionId",
                table: "Basistrinn",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_BasistrinnGrunntype_GrunntypeId",
                table: "BasistrinnGrunntype",
                column: "GrunntypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BasistrinnTrinn_TrinnId",
                table: "BasistrinnTrinn",
                column: "TrinnId");

            migrationBuilder.CreateIndex(
                name: "IX_Grunntype_HovedtypeId",
                table: "Grunntype",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Grunntype_VersionId",
                table: "Grunntype",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_GrunntypeKartleggingsenhet_KartleggingsenhetId",
                table: "GrunntypeKartleggingsenhet",
                column: "KartleggingsenhetId");

            migrationBuilder.CreateIndex(
                name: "IX_GrunntypeMiljovariabel_MiljovariabelId",
                table: "GrunntypeMiljovariabel",
                column: "MiljovariabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtype_HovedtypegruppeId",
                table: "Hovedtype",
                column: "HovedtypegruppeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtype_VersionId",
                table: "Hovedtype",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtypegruppe_NatursystemId",
                table: "Hovedtypegruppe",
                column: "NatursystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtypegruppe_VersionId",
                table: "Hovedtypegruppe",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Kartleggingsenhet_HovedtypeId",
                table: "Kartleggingsenhet",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Kartleggingsenhet_VersionId",
                table: "Kartleggingsenhet",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_GrunntypeId",
                table: "Kode",
                column: "GrunntypeId",
                unique: true,
                filter: "[GrunntypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_HovedtypegruppeId",
                table: "Kode",
                column: "HovedtypegruppeId",
                unique: true,
                filter: "[HovedtypegruppeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_HovedtypeId",
                table: "Kode",
                column: "HovedtypeId",
                unique: true,
                filter: "[HovedtypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_KartleggingsenhetId",
                table: "Kode",
                column: "KartleggingsenhetId",
                unique: true,
                filter: "[KartleggingsenhetId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_NatursystemId",
                table: "Kode",
                column: "NatursystemId",
                unique: true,
                filter: "[NatursystemId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_TrinnId",
                table: "Kode",
                column: "TrinnId",
                unique: true,
                filter: "[TrinnId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Kode_VersionId",
                table: "Kode",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_LKMKode_VersionId",
                table: "LKMKode",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_HovedtypeId",
                table: "Miljovariabel",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_KodeId",
                table: "Miljovariabel",
                column: "KodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_VersionId",
                table: "Miljovariabel",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Natursystem_VersionId",
                table: "Natursystem",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trinn_MiljovariabelId",
                table: "Trinn",
                column: "MiljovariabelId");

            migrationBuilder.CreateIndex(
                name: "IX_Trinn_VersionId",
                table: "Trinn",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_VariasjonKode_VarietyLevel1Code_VarietyLevelId",
                table: "VariasjonKode",
                column: "VarietyLevel1Code_VarietyLevelId",
                unique: true,
                filter: "[VarietyLevel1Code_VarietyLevelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VariasjonKode_VarietyLevel2Code_VarietyLevelId",
                table: "VariasjonKode",
                column: "VarietyLevel2Code_VarietyLevelId",
                unique: true,
                filter: "[VarietyLevel2Code_VarietyLevelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VariasjonKode_VarietyLevel3Code_VarietyLevelId",
                table: "VariasjonKode",
                column: "VarietyLevel3Code_VarietyLevelId",
                unique: true,
                filter: "[VarietyLevel3Code_VarietyLevelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VariasjonKode_VarietyLevel4Code_VarietyLevelId",
                table: "VariasjonKode",
                column: "VarietyLevel4Code_VarietyLevelId",
                unique: true,
                filter: "[VarietyLevel4Code_VarietyLevelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VariasjonKode_VarietyLevel5Code_VarietyLevelId",
                table: "VariasjonKode",
                column: "VarietyLevel5Code_VarietyLevelId",
                unique: true,
                filter: "[VarietyLevel5Code_VarietyLevelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VariasjonKode_VarietyLevelId",
                table: "VariasjonKode",
                column: "VarietyLevelId",
                unique: true,
                filter: "[VarietyLevelId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VariasjonKode_VersionId",
                table: "VariasjonKode",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel0_VersionId",
                table: "VarietyLevel0",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel1_OverordnetKodeId",
                table: "VarietyLevel1",
                column: "OverordnetKodeId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel1_VersionId",
                table: "VarietyLevel1",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel2_OverordnetKodeId",
                table: "VarietyLevel2",
                column: "OverordnetKodeId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel2_VersionId",
                table: "VarietyLevel2",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel3_OverordnetKodeId",
                table: "VarietyLevel3",
                column: "OverordnetKodeId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel3_VersionId",
                table: "VarietyLevel3",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel4_OverordnetKodeId",
                table: "VarietyLevel4",
                column: "OverordnetKodeId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel4_VersionId",
                table: "VarietyLevel4",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel5_OverordnetKodeId",
                table: "VarietyLevel5",
                column: "OverordnetKodeId");

            migrationBuilder.CreateIndex(
                name: "IX_VarietyLevel5_VersionId",
                table: "VarietyLevel5",
                column: "VersionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasistrinnGrunntype");

            migrationBuilder.DropTable(
                name: "BasistrinnTrinn");

            migrationBuilder.DropTable(
                name: "GrunntypeKartleggingsenhet");

            migrationBuilder.DropTable(
                name: "GrunntypeMiljovariabel");

            migrationBuilder.DropTable(
                name: "Kode");

            migrationBuilder.DropTable(
                name: "VariasjonKode");

            migrationBuilder.DropTable(
                name: "Basistrinn");

            migrationBuilder.DropTable(
                name: "Grunntype");

            migrationBuilder.DropTable(
                name: "Kartleggingsenhet");

            migrationBuilder.DropTable(
                name: "Trinn");

            migrationBuilder.DropTable(
                name: "VarietyLevel5");

            migrationBuilder.DropTable(
                name: "Miljovariabel");

            migrationBuilder.DropTable(
                name: "VarietyLevel4");

            migrationBuilder.DropTable(
                name: "Hovedtype");

            migrationBuilder.DropTable(
                name: "LKMKode");

            migrationBuilder.DropTable(
                name: "VarietyLevel3");

            migrationBuilder.DropTable(
                name: "Hovedtypegruppe");

            migrationBuilder.DropTable(
                name: "VarietyLevel2");

            migrationBuilder.DropTable(
                name: "Natursystem");

            migrationBuilder.DropTable(
                name: "VarietyLevel1");

            migrationBuilder.DropTable(
                name: "VarietyLevel0");

            migrationBuilder.DropTable(
                name: "NinVersion");
        }
    }
}