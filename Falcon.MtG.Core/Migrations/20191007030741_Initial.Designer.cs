﻿// <auto-generated />
using System;
using Falcon.MtG;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Falcon.MtG.Migrations
{
    [DbContext(typeof(MtGDBContext))]
    [Migration("20191007030741_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Falcon.MtG.Artist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("Falcon.MtG.Block", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("Falcon.MtG.Border", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Borders");
                });

            modelBuilder.Entity("Falcon.MtG.Card", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("CMC")
                        .HasColumnType("float");

                    b.Property<int?>("EDHRECRank")
                        .HasColumnType("int");

                    b.Property<int>("LayoutID")
                        .HasColumnType("int");

                    b.Property<string>("Loyalty")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("MainSideID")
                        .HasColumnType("int");

                    b.Property<string>("ManaCost")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OracleText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Power")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Side")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Toughness")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TypeLine")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("LayoutID");

                    b.HasIndex("MainSideID");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Falcon.MtG.CardCardType", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("CardTypeID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "CardTypeID");

                    b.HasIndex("CardTypeID");

                    b.ToTable("CardCardTypes");
                });

            modelBuilder.Entity("Falcon.MtG.CardColor", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("ColorID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "ColorID");

                    b.HasIndex("ColorID");

                    b.ToTable("CardColors");
                });

            modelBuilder.Entity("Falcon.MtG.CardColorIdentity", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("ColorID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "ColorID");

                    b.HasIndex("ColorID");

                    b.ToTable("CardColorIdentities");
                });

            modelBuilder.Entity("Falcon.MtG.CardKeyword", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("KeywordID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "KeywordID");

                    b.HasIndex("KeywordID");

                    b.ToTable("CardKeywords");
                });

            modelBuilder.Entity("Falcon.MtG.CardSubtype", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("SubtypeID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "SubtypeID");

                    b.HasIndex("SubtypeID");

                    b.ToTable("CardSubtypes");
                });

            modelBuilder.Entity("Falcon.MtG.CardSupertype", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("SupertypeID")
                        .HasColumnType("int");

                    b.Property<int?>("SubtypeID")
                        .HasColumnType("int");

                    b.Property<int?>("SupertypeID1")
                        .HasColumnType("int");

                    b.HasKey("CardID", "SupertypeID");

                    b.HasIndex("SubtypeID");

                    b.HasIndex("SupertypeID");

                    b.HasIndex("SupertypeID1");

                    b.ToTable("CardSupertypes");
                });

            modelBuilder.Entity("Falcon.MtG.CardType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("CardTypes");
                });

            modelBuilder.Entity("Falcon.MtG.CardTypeSubtype", b =>
                {
                    b.Property<int>("CardTypeID")
                        .HasColumnType("int");

                    b.Property<int>("SubtypeID")
                        .HasColumnType("int");

                    b.HasKey("CardTypeID", "SubtypeID");

                    b.HasIndex("SubtypeID");

                    b.ToTable("CardTypeSubtypes");
                });

            modelBuilder.Entity("Falcon.MtG.CardTypeSupertype", b =>
                {
                    b.Property<int>("CardTypeID")
                        .HasColumnType("int");

                    b.Property<int>("SupertypeID")
                        .HasColumnType("int");

                    b.HasKey("CardTypeID", "SupertypeID");

                    b.HasIndex("SupertypeID");

                    b.ToTable("CardTypeSupertypes");
                });

            modelBuilder.Entity("Falcon.MtG.Color", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("BasicLandName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("Falcon.MtG.ExceptionLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Outcome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestJson")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("Falcon.MtG.Frame", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Frames");
                });

            modelBuilder.Entity("Falcon.MtG.Keyword", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("Falcon.MtG.Layout", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Layouts");
                });

            modelBuilder.Entity("Falcon.MtG.Legality", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<string>("Format")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Legal")
                        .HasColumnType("bit");

                    b.Property<bool>("LegalAsCommander")
                        .HasColumnType("bit");

                    b.HasKey("ID");

                    b.HasIndex("CardID");

                    b.ToTable("Legalities");
                });

            modelBuilder.Entity("Falcon.MtG.Pricing", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Foil")
                        .HasColumnType("bit");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("PrintingID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("PrintingID");

                    b.ToTable("Pricings");
                });

            modelBuilder.Entity("Falcon.MtG.Printing", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ArtistID")
                        .HasColumnType("int");

                    b.Property<int>("BorderID")
                        .HasColumnType("int");

                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<string>("CollectorNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlavorText")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FrameID")
                        .HasColumnType("int");

                    b.Property<int>("MultiverseId")
                        .HasColumnType("int");

                    b.Property<int>("RarityID")
                        .HasColumnType("int");

                    b.Property<int>("SetID")
                        .HasColumnType("int");

                    b.Property<string>("Side")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WatermarkID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("ArtistID");

                    b.HasIndex("BorderID");

                    b.HasIndex("CardID");

                    b.HasIndex("FrameID");

                    b.HasIndex("RarityID");

                    b.HasIndex("SetID");

                    b.HasIndex("WatermarkID");

                    b.ToTable("Printings");
                });

            modelBuilder.Entity("Falcon.MtG.Rarity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Rarities");
                });

            modelBuilder.Entity("Falcon.MtG.Set", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BlockID")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("KeyruneCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SetTypeID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("BlockID");

                    b.HasIndex("SetTypeID");

                    b.ToTable("Sets");
                });

            modelBuilder.Entity("Falcon.MtG.SetType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("SetTypes");
                });

            modelBuilder.Entity("Falcon.MtG.Subtype", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Subtypes");
                });

            modelBuilder.Entity("Falcon.MtG.Supertype", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Supertypes");
                });

            modelBuilder.Entity("Falcon.MtG.Watermark", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Watermarks");
                });

            modelBuilder.Entity("Falcon.MtG.Card", b =>
                {
                    b.HasOne("Falcon.MtG.Layout", "Layout")
                        .WithMany("Cards")
                        .HasForeignKey("LayoutID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Card", "MainSide")
                        .WithMany("OtherSides")
                        .HasForeignKey("MainSideID");
                });

            modelBuilder.Entity("Falcon.MtG.CardCardType", b =>
                {
                    b.HasOne("Falcon.MtG.Card", "Card")
                        .WithMany("Types")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.CardType", "CardType")
                        .WithMany("Cards")
                        .HasForeignKey("CardTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.CardColor", b =>
                {
                    b.HasOne("Falcon.MtG.Card", "Card")
                        .WithMany("Colors")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Color", "Color")
                        .WithMany()
                        .HasForeignKey("ColorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.CardColorIdentity", b =>
                {
                    b.HasOne("Falcon.MtG.Card", "Card")
                        .WithMany("ColorIdentity")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Color", "Color")
                        .WithMany()
                        .HasForeignKey("ColorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.CardKeyword", b =>
                {
                    b.HasOne("Falcon.MtG.Card", "Card")
                        .WithMany("Keywords")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Keyword", "Keyword")
                        .WithMany("Cards")
                        .HasForeignKey("KeywordID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.CardSubtype", b =>
                {
                    b.HasOne("Falcon.MtG.Card", "Card")
                        .WithMany("Subtypes")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Subtype", "Subtype")
                        .WithMany()
                        .HasForeignKey("SubtypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.CardSupertype", b =>
                {
                    b.HasOne("Falcon.MtG.Card", "Card")
                        .WithMany("Supertypes")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Subtype", null)
                        .WithMany("Cards")
                        .HasForeignKey("SubtypeID");

                    b.HasOne("Falcon.MtG.Supertype", "Supertype")
                        .WithMany()
                        .HasForeignKey("SupertypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Supertype", null)
                        .WithMany("Cards")
                        .HasForeignKey("SupertypeID1");
                });

            modelBuilder.Entity("Falcon.MtG.CardTypeSubtype", b =>
                {
                    b.HasOne("Falcon.MtG.CardType", "CardType")
                        .WithMany("Subtypes")
                        .HasForeignKey("CardTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Subtype", "Subtype")
                        .WithMany("Types")
                        .HasForeignKey("SubtypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.CardTypeSupertype", b =>
                {
                    b.HasOne("Falcon.MtG.CardType", "CardType")
                        .WithMany("Supertypes")
                        .HasForeignKey("CardTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Supertype", "Supertype")
                        .WithMany("Types")
                        .HasForeignKey("SupertypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.Legality", b =>
                {
                    b.HasOne("Falcon.MtG.Card", "Card")
                        .WithMany("Legalities")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.Pricing", b =>
                {
                    b.HasOne("Falcon.MtG.Printing", "Printing")
                        .WithMany("Pricings")
                        .HasForeignKey("PrintingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Falcon.MtG.Printing", b =>
                {
                    b.HasOne("Falcon.MtG.Artist", "Artist")
                        .WithMany("Printings")
                        .HasForeignKey("ArtistID");

                    b.HasOne("Falcon.MtG.Border", "Border")
                        .WithMany("Printings")
                        .HasForeignKey("BorderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Card", "Card")
                        .WithMany("Printings")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Frame", "Frame")
                        .WithMany("Printings")
                        .HasForeignKey("FrameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Rarity", "Rarity")
                        .WithMany("Printings")
                        .HasForeignKey("RarityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Set", "Set")
                        .WithMany("Printings")
                        .HasForeignKey("SetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Watermark", "Watermark")
                        .WithMany("Printings")
                        .HasForeignKey("WatermarkID");
                });

            modelBuilder.Entity("Falcon.MtG.Set", b =>
                {
                    b.HasOne("Falcon.MtG.Block", "Block")
                        .WithMany("Sets")
                        .HasForeignKey("BlockID");

                    b.HasOne("Falcon.MtG.SetType", "SetType")
                        .WithMany("Sets")
                        .HasForeignKey("SetTypeID");
                });
#pragma warning restore 612, 618
        }
    }
}