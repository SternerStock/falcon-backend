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
    [Migration("20210111043604_v5_2")]
    partial class v5_2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Falcon.MtG.Models.Sql.AlsoKnownAs", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("AlsoKnownAs");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Artist", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Block", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Blocks");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Border", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Borders");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Card", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<double>("CMC")
                        .HasColumnType("float");

                    b.Property<string>("CockatriceName")
                        .HasColumnType("nvarchar(max)");

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

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardCardType", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("CardTypeID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "CardTypeID");

                    b.HasIndex("CardTypeID");

                    b.ToTable("CardCardTypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardColor", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("ColorID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "ColorID");

                    b.HasIndex("ColorID");

                    b.ToTable("CardColors");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardColorIdentity", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("ColorID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "ColorID");

                    b.HasIndex("ColorID");

                    b.ToTable("CardColorIdentities");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardKeyword", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("KeywordID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "KeywordID");

                    b.HasIndex("KeywordID");

                    b.ToTable("CardKeywords");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardSubtype", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("SubtypeID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "SubtypeID");

                    b.HasIndex("SubtypeID");

                    b.ToTable("CardSubtypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardSupertype", b =>
                {
                    b.Property<int>("CardID")
                        .HasColumnType("int");

                    b.Property<int>("SupertypeID")
                        .HasColumnType("int");

                    b.HasKey("CardID", "SupertypeID");

                    b.HasIndex("SupertypeID");

                    b.ToTable("CardSupertypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("CardTypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardTypeSubtype", b =>
                {
                    b.Property<int>("CardTypeID")
                        .HasColumnType("int");

                    b.Property<int>("SubtypeID")
                        .HasColumnType("int");

                    b.HasKey("CardTypeID", "SubtypeID");

                    b.HasIndex("SubtypeID");

                    b.ToTable("CardTypeSubtypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardTypeSupertype", b =>
                {
                    b.Property<int>("CardTypeID")
                        .HasColumnType("int");

                    b.Property<int>("SupertypeID")
                        .HasColumnType("int");

                    b.HasKey("CardTypeID", "SupertypeID");

                    b.HasIndex("SupertypeID");

                    b.ToTable("CardTypeSupertypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Color", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("BasicLandName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.ExceptionLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Outcome")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RequestJson")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Frame", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Frames");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Keyword", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Keywords");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Layout", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Layouts");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Legality", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

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

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Pricing", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

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

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Printing", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

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

                    b.Property<Guid>("UUID")
                        .HasColumnType("uniqueidentifier");

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

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Rarity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Rarities");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Set", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

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

            modelBuilder.Entity("Falcon.MtG.Models.Sql.SetType", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("SetTypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Subtype", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Subtypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Supertype", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Supertypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Watermark", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Watermarks");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Card", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Layout", "Layout")
                        .WithMany("Cards")
                        .HasForeignKey("LayoutID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Card", "MainSide")
                        .WithMany("OtherSides")
                        .HasForeignKey("MainSideID");

                    b.Navigation("Layout");

                    b.Navigation("MainSide");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardCardType", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Card", "Card")
                        .WithMany("Types")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.CardType", "CardType")
                        .WithMany("Cards")
                        .HasForeignKey("CardTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("CardType");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardColor", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Card", "Card")
                        .WithMany("Colors")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Color", "Color")
                        .WithMany()
                        .HasForeignKey("ColorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Color");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardColorIdentity", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Card", "Card")
                        .WithMany("ColorIdentity")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Color", "Color")
                        .WithMany()
                        .HasForeignKey("ColorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Color");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardKeyword", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Card", "Card")
                        .WithMany("Keywords")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Keyword", "Keyword")
                        .WithMany("Cards")
                        .HasForeignKey("KeywordID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Keyword");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardSubtype", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Card", "Card")
                        .WithMany("Subtypes")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Subtype", "Subtype")
                        .WithMany("Cards")
                        .HasForeignKey("SubtypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Subtype");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardSupertype", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Card", "Card")
                        .WithMany("Supertypes")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Supertype", "Supertype")
                        .WithMany("Cards")
                        .HasForeignKey("SupertypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Supertype");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardTypeSubtype", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.CardType", "CardType")
                        .WithMany("Subtypes")
                        .HasForeignKey("CardTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Subtype", "Subtype")
                        .WithMany("Types")
                        .HasForeignKey("SubtypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CardType");

                    b.Navigation("Subtype");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardTypeSupertype", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.CardType", "CardType")
                        .WithMany("Supertypes")
                        .HasForeignKey("CardTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Supertype", "Supertype")
                        .WithMany("Types")
                        .HasForeignKey("SupertypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CardType");

                    b.Navigation("Supertype");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Legality", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Card", "Card")
                        .WithMany("Legalities")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Pricing", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Printing", "Printing")
                        .WithMany("Pricings")
                        .HasForeignKey("PrintingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Printing");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Printing", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Artist", "Artist")
                        .WithMany("Printings")
                        .HasForeignKey("ArtistID");

                    b.HasOne("Falcon.MtG.Models.Sql.Border", "Border")
                        .WithMany("Printings")
                        .HasForeignKey("BorderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Card", "Card")
                        .WithMany("Printings")
                        .HasForeignKey("CardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Frame", "Frame")
                        .WithMany("Printings")
                        .HasForeignKey("FrameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Rarity", "Rarity")
                        .WithMany("Printings")
                        .HasForeignKey("RarityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Set", "Set")
                        .WithMany("Printings")
                        .HasForeignKey("SetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Falcon.MtG.Models.Sql.Watermark", "Watermark")
                        .WithMany("Printings")
                        .HasForeignKey("WatermarkID");

                    b.Navigation("Artist");

                    b.Navigation("Border");

                    b.Navigation("Card");

                    b.Navigation("Frame");

                    b.Navigation("Rarity");

                    b.Navigation("Set");

                    b.Navigation("Watermark");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Set", b =>
                {
                    b.HasOne("Falcon.MtG.Models.Sql.Block", "Block")
                        .WithMany("Sets")
                        .HasForeignKey("BlockID");

                    b.HasOne("Falcon.MtG.Models.Sql.SetType", "SetType")
                        .WithMany("Sets")
                        .HasForeignKey("SetTypeID");

                    b.Navigation("Block");

                    b.Navigation("SetType");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Artist", b =>
                {
                    b.Navigation("Printings");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Block", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Border", b =>
                {
                    b.Navigation("Printings");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Card", b =>
                {
                    b.Navigation("ColorIdentity");

                    b.Navigation("Colors");

                    b.Navigation("Keywords");

                    b.Navigation("Legalities");

                    b.Navigation("OtherSides");

                    b.Navigation("Printings");

                    b.Navigation("Subtypes");

                    b.Navigation("Supertypes");

                    b.Navigation("Types");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.CardType", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Subtypes");

                    b.Navigation("Supertypes");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Frame", b =>
                {
                    b.Navigation("Printings");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Keyword", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Layout", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Printing", b =>
                {
                    b.Navigation("Pricings");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Rarity", b =>
                {
                    b.Navigation("Printings");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Set", b =>
                {
                    b.Navigation("Printings");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.SetType", b =>
                {
                    b.Navigation("Sets");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Subtype", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Types");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Supertype", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Types");
                });

            modelBuilder.Entity("Falcon.MtG.Models.Sql.Watermark", b =>
                {
                    b.Navigation("Printings");
                });
#pragma warning restore 612, 618
        }
    }
}
