using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Falcon.MtG.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Borders",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borders", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CardTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    BasicLandName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Frames",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frames", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Keywords",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keywords", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Layouts",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Layouts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestJson = table.Column<string>(nullable: true),
                    Outcome = table.Column<string>(nullable: true),
                    Exception = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rarities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rarities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SetTypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Subtypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Supertypes",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supertypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Watermarks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watermarks", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ManaCost = table.Column<string>(nullable: true),
                    CMC = table.Column<double>(nullable: false),
                    TypeLine = table.Column<string>(nullable: true),
                    OracleText = table.Column<string>(nullable: true),
                    Power = table.Column<string>(nullable: true),
                    Toughness = table.Column<string>(nullable: true),
                    Loyalty = table.Column<string>(nullable: true),
                    LayoutID = table.Column<int>(nullable: false),
                    MainSideID = table.Column<int>(nullable: true),
                    Side = table.Column<string>(nullable: true),
                    EDHRECRank = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cards_Layouts_LayoutID",
                        column: x => x.LayoutID,
                        principalTable: "Layouts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Cards_MainSideID",
                        column: x => x.MainSideID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sets",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    KeyruneCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    SetTypeID = table.Column<int>(nullable: true),
                    BlockID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sets", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sets_Blocks_BlockID",
                        column: x => x.BlockID,
                        principalTable: "Blocks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sets_SetTypes_SetTypeID",
                        column: x => x.SetTypeID,
                        principalTable: "SetTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CardTypeSubtypes",
                columns: table => new
                {
                    CardTypeID = table.Column<int>(nullable: false),
                    SubtypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTypeSubtypes", x => new { x.CardTypeID, x.SubtypeID });
                    table.ForeignKey(
                        name: "FK_CardTypeSubtypes_CardTypes_CardTypeID",
                        column: x => x.CardTypeID,
                        principalTable: "CardTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardTypeSubtypes_Subtypes_SubtypeID",
                        column: x => x.SubtypeID,
                        principalTable: "Subtypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardTypeSupertypes",
                columns: table => new
                {
                    CardTypeID = table.Column<int>(nullable: false),
                    SupertypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTypeSupertypes", x => new { x.CardTypeID, x.SupertypeID });
                    table.ForeignKey(
                        name: "FK_CardTypeSupertypes_CardTypes_CardTypeID",
                        column: x => x.CardTypeID,
                        principalTable: "CardTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardTypeSupertypes_Supertypes_SupertypeID",
                        column: x => x.SupertypeID,
                        principalTable: "Supertypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardCardTypes",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false),
                    CardTypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardCardTypes", x => new { x.CardID, x.CardTypeID });
                    table.ForeignKey(
                        name: "FK_CardCardTypes_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardCardTypes_CardTypes_CardTypeID",
                        column: x => x.CardTypeID,
                        principalTable: "CardTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardColorIdentities",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false),
                    ColorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardColorIdentities", x => new { x.CardID, x.ColorID });
                    table.ForeignKey(
                        name: "FK_CardColorIdentities_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardColorIdentities_Colors_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardColors",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false),
                    ColorID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardColors", x => new { x.CardID, x.ColorID });
                    table.ForeignKey(
                        name: "FK_CardColors_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardColors_Colors_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardKeywords",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false),
                    KeywordID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardKeywords", x => new { x.CardID, x.KeywordID });
                    table.ForeignKey(
                        name: "FK_CardKeywords_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardKeywords_Keywords_KeywordID",
                        column: x => x.KeywordID,
                        principalTable: "Keywords",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardSubtypes",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false),
                    SubtypeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSubtypes", x => new { x.CardID, x.SubtypeID });
                    table.ForeignKey(
                        name: "FK_CardSubtypes_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardSubtypes_Subtypes_SubtypeID",
                        column: x => x.SubtypeID,
                        principalTable: "Subtypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CardSupertypes",
                columns: table => new
                {
                    CardID = table.Column<int>(nullable: false),
                    SupertypeID = table.Column<int>(nullable: false),
                    SubtypeID = table.Column<int>(nullable: true),
                    SupertypeID1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSupertypes", x => new { x.CardID, x.SupertypeID });
                    table.ForeignKey(
                        name: "FK_CardSupertypes_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardSupertypes_Subtypes_SubtypeID",
                        column: x => x.SubtypeID,
                        principalTable: "Subtypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CardSupertypes_Supertypes_SupertypeID",
                        column: x => x.SupertypeID,
                        principalTable: "Supertypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CardSupertypes_Supertypes_SupertypeID1",
                        column: x => x.SupertypeID1,
                        principalTable: "Supertypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Legalities",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Format = table.Column<string>(nullable: true),
                    LegalAsCommander = table.Column<bool>(nullable: false),
                    Legal = table.Column<bool>(nullable: false),
                    CardID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Legalities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Legalities_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Printings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MultiverseId = table.Column<int>(nullable: false),
                    FlavorText = table.Column<string>(nullable: true),
                    CollectorNumber = table.Column<string>(nullable: true),
                    Side = table.Column<string>(nullable: true),
                    ArtistID = table.Column<int>(nullable: true),
                    WatermarkID = table.Column<int>(nullable: true),
                    FrameID = table.Column<int>(nullable: false),
                    RarityID = table.Column<int>(nullable: false),
                    BorderID = table.Column<int>(nullable: false),
                    SetID = table.Column<int>(nullable: false),
                    CardID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Printings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Printings_Artists_ArtistID",
                        column: x => x.ArtistID,
                        principalTable: "Artists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Printings_Borders_BorderID",
                        column: x => x.BorderID,
                        principalTable: "Borders",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Printings_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Printings_Frames_FrameID",
                        column: x => x.FrameID,
                        principalTable: "Frames",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Printings_Rarities_RarityID",
                        column: x => x.RarityID,
                        principalTable: "Rarities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Printings_Sets_SetID",
                        column: x => x.SetID,
                        principalTable: "Sets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Printings_Watermarks_WatermarkID",
                        column: x => x.WatermarkID,
                        principalTable: "Watermarks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pricings",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrintingID = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Foil = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pricings", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pricings_Printings_PrintingID",
                        column: x => x.PrintingID,
                        principalTable: "Printings",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CardCardTypes_CardTypeID",
                table: "CardCardTypes",
                column: "CardTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CardColorIdentities_ColorID",
                table: "CardColorIdentities",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_CardColors_ColorID",
                table: "CardColors",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_CardKeywords_KeywordID",
                table: "CardKeywords",
                column: "KeywordID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_LayoutID",
                table: "Cards",
                column: "LayoutID");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_MainSideID",
                table: "Cards",
                column: "MainSideID");

            migrationBuilder.CreateIndex(
                name: "IX_CardSubtypes_SubtypeID",
                table: "CardSubtypes",
                column: "SubtypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CardSupertypes_SubtypeID",
                table: "CardSupertypes",
                column: "SubtypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CardSupertypes_SupertypeID",
                table: "CardSupertypes",
                column: "SupertypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CardSupertypes_SupertypeID1",
                table: "CardSupertypes",
                column: "SupertypeID1");

            migrationBuilder.CreateIndex(
                name: "IX_CardTypeSubtypes_SubtypeID",
                table: "CardTypeSubtypes",
                column: "SubtypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CardTypeSupertypes_SupertypeID",
                table: "CardTypeSupertypes",
                column: "SupertypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Legalities_CardID",
                table: "Legalities",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Pricings_PrintingID",
                table: "Pricings",
                column: "PrintingID");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_ArtistID",
                table: "Printings",
                column: "ArtistID");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_BorderID",
                table: "Printings",
                column: "BorderID");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_CardID",
                table: "Printings",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_FrameID",
                table: "Printings",
                column: "FrameID");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_RarityID",
                table: "Printings",
                column: "RarityID");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_SetID",
                table: "Printings",
                column: "SetID");

            migrationBuilder.CreateIndex(
                name: "IX_Printings_WatermarkID",
                table: "Printings",
                column: "WatermarkID");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_BlockID",
                table: "Sets",
                column: "BlockID");

            migrationBuilder.CreateIndex(
                name: "IX_Sets_SetTypeID",
                table: "Sets",
                column: "SetTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardCardTypes");

            migrationBuilder.DropTable(
                name: "CardColorIdentities");

            migrationBuilder.DropTable(
                name: "CardColors");

            migrationBuilder.DropTable(
                name: "CardKeywords");

            migrationBuilder.DropTable(
                name: "CardSubtypes");

            migrationBuilder.DropTable(
                name: "CardSupertypes");

            migrationBuilder.DropTable(
                name: "CardTypeSubtypes");

            migrationBuilder.DropTable(
                name: "CardTypeSupertypes");

            migrationBuilder.DropTable(
                name: "Legalities");

            migrationBuilder.DropTable(
                name: "LogEntries");

            migrationBuilder.DropTable(
                name: "Pricings");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Keywords");

            migrationBuilder.DropTable(
                name: "Subtypes");

            migrationBuilder.DropTable(
                name: "CardTypes");

            migrationBuilder.DropTable(
                name: "Supertypes");

            migrationBuilder.DropTable(
                name: "Printings");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Borders");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Frames");

            migrationBuilder.DropTable(
                name: "Rarities");

            migrationBuilder.DropTable(
                name: "Sets");

            migrationBuilder.DropTable(
                name: "Watermarks");

            migrationBuilder.DropTable(
                name: "Layouts");

            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DropTable(
                name: "SetTypes");
        }
    }
}
