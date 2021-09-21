using Microsoft.EntityFrameworkCore.Migrations;

namespace NiN.Database.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LKMKode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KodeId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKMKode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Natursystem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Kategori = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Natursystem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Basistrinn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Kode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LKMKodeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Basistrinn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Basistrinn_LKMKode_LKMKodeId",
                        column: x => x.LKMKodeId,
                        principalTable: "LKMKode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hovedtypegruppe",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NatursystemId = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "NatursystemKode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NatursystemId = table.Column<int>(type: "int", nullable: false),
                    KodeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kategori = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NatursystemKode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NatursystemKode_Natursystem_NatursystemId",
                        column: x => x.NatursystemId,
                        principalTable: "Natursystem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hovedtype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HovedtypegruppeId = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "HovedtypegruppeKode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HovedtypegruppeId = table.Column<int>(type: "int", nullable: false),
                    KodeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kategori = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HovedtypegruppeKode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HovedtypegruppeKode_Hovedtypegruppe_HovedtypegruppeId",
                        column: x => x.HovedtypegruppeId,
                        principalTable: "Hovedtypegruppe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grunntype",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "HovedtypeKode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HovedtypeKeyId = table.Column<int>(type: "int", nullable: false),
                    KodeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kategori = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HovedtypeKode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HovedtypeKode_Hovedtype_HovedtypeKeyId",
                        column: x => x.HovedtypeKeyId,
                        principalTable: "Hovedtype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kartleggingsenhet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Malestokk = table.Column<int>(type: "int", nullable: false),
                    KodeId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "Miljovariabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Kode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LkmKategori = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    HovedtypeId = table.Column<int>(type: "int", nullable: true)
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
                });

            migrationBuilder.CreateTable(
                name: "GrunntypeKode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrunntypeId = table.Column<int>(type: "int", nullable: false),
                    KodeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Definisjon = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Kategori = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrunntypeKode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrunntypeKode_Grunntype_GrunntypeId",
                        column: x => x.GrunntypeId,
                        principalTable: "Grunntype",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trinn",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Navn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    KodeId = table.Column<int>(type: "int", nullable: true),
                    MiljovariabelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trinn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trinn_LKMKode_KodeId",
                        column: x => x.KodeId,
                        principalTable: "LKMKode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trinn_Miljovariabel_MiljovariabelId",
                        column: x => x.MiljovariabelId,
                        principalTable: "Miljovariabel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Basistrinn_LKMKodeId",
                table: "Basistrinn",
                column: "LKMKodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Grunntype_HovedtypeId",
                table: "Grunntype",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrunntypeKode_GrunntypeId",
                table: "GrunntypeKode",
                column: "GrunntypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtype_HovedtypegruppeId",
                table: "Hovedtype",
                column: "HovedtypegruppeId");

            migrationBuilder.CreateIndex(
                name: "IX_Hovedtypegruppe_NatursystemId",
                table: "Hovedtypegruppe",
                column: "NatursystemId");

            migrationBuilder.CreateIndex(
                name: "IX_HovedtypegruppeKode_HovedtypegruppeId",
                table: "HovedtypegruppeKode",
                column: "HovedtypegruppeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HovedtypeKode_HovedtypeKeyId",
                table: "HovedtypeKode",
                column: "HovedtypeKeyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kartleggingsenhet_HovedtypeId",
                table: "Kartleggingsenhet",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Miljovariabel_HovedtypeId",
                table: "Miljovariabel",
                column: "HovedtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NatursystemKode_NatursystemId",
                table: "NatursystemKode",
                column: "NatursystemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trinn_KodeId",
                table: "Trinn",
                column: "KodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Trinn_MiljovariabelId",
                table: "Trinn",
                column: "MiljovariabelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Basistrinn");

            migrationBuilder.DropTable(
                name: "GrunntypeKode");

            migrationBuilder.DropTable(
                name: "HovedtypegruppeKode");

            migrationBuilder.DropTable(
                name: "HovedtypeKode");

            migrationBuilder.DropTable(
                name: "Kartleggingsenhet");

            migrationBuilder.DropTable(
                name: "NatursystemKode");

            migrationBuilder.DropTable(
                name: "Trinn");

            migrationBuilder.DropTable(
                name: "Grunntype");

            migrationBuilder.DropTable(
                name: "LKMKode");

            migrationBuilder.DropTable(
                name: "Miljovariabel");

            migrationBuilder.DropTable(
                name: "Hovedtype");

            migrationBuilder.DropTable(
                name: "Hovedtypegruppe");

            migrationBuilder.DropTable(
                name: "Natursystem");
        }
    }
}
