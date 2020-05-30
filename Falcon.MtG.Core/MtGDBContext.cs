namespace Falcon.MtG
{
    using System;
    using System.IO;
    using Falcon.MtG.Models.Sql;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class MtGDBContext : DbContext
    {
        public MtGDBContext()
        {
        }

        public MtGDBContext(DbContextOptions<MtGDBContext> options)
            : base(options)
        {
        }

        public DbSet<AlsoKnownAs> AlsoKnownAs { get; set; }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Border> Borders { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ExceptionLog> LogEntries { get; set; }
        public DbSet<Frame> Frames { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<Layout> Layouts { get; set; }
        public DbSet<Legality> Legalities { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Printing> Printings { get; set; }
        public DbSet<Rarity> Rarities { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<SetType> SetTypes { get; set; }
        public DbSet<Subtype> Subtypes { get; set; }
        public DbSet<Supertype> Supertypes { get; set; }
        public DbSet<Watermark> Watermarks { get; set; }

        public DbSet<CardCardType> CardCardTypes { get; set; }
        public DbSet<CardColor> CardColors { get; set; }
        public DbSet<CardColorIdentity> CardColorIdentities { get; set; }
        public DbSet<CardKeyword> CardKeywords { get; set; }
        public DbSet<CardSubtype> CardSubtypes { get; set; }
        public DbSet<CardSupertype> CardSupertypes { get; set; }
        public DbSet<CardTypeSubtype> CardTypeSubtypes { get; set; }
        public DbSet<CardTypeSupertype> CardTypeSupertypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", false)
                .Build();

            var connectionString = configuration.GetConnectionString("MtGDBContext");
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CardCardType>()
                .HasKey(ct => new { ct.CardID, ct.CardTypeID });
            modelBuilder.Entity<CardCardType>()
                .HasOne(ct => ct.Card)
                .WithMany(c => c.Types)
                .HasForeignKey(ct => ct.CardID);
            modelBuilder.Entity<CardCardType>()
                .HasOne(ct => ct.CardType)
                .WithMany(t => t.Cards)
                .HasForeignKey(ct => ct.CardTypeID);

            modelBuilder.Entity<CardColor>()
                .HasKey(cc => new { cc.CardID, cc.ColorID });
            modelBuilder.Entity<CardColor>()
                .HasOne(cc => cc.Card)
                .WithMany(c => c.Colors)
                .HasForeignKey(cc => cc.CardID);
            modelBuilder.Entity<CardColor>()
                .HasOne(cc => cc.Color)
                .WithMany()
                .HasForeignKey(cc => cc.ColorID);

            modelBuilder.Entity<CardColorIdentity>()
                .HasKey(cc => new { cc.CardID, cc.ColorID });
            modelBuilder.Entity<CardColorIdentity>()
                .HasOne(cc => cc.Card)
                .WithMany(c => c.ColorIdentity)
                .HasForeignKey(cc => cc.CardID);
            modelBuilder.Entity<CardColorIdentity>()
                .HasOne(cc => cc.Color)
                .WithMany()
                .HasForeignKey(cc => cc.ColorID);

            modelBuilder.Entity<CardKeyword>()
                .HasKey(ck => new { ck.CardID, ck.KeywordID });
            modelBuilder.Entity<CardKeyword>()
                .HasOne(ck => ck.Card)
                .WithMany(c => c.Keywords)
                .HasForeignKey(ck => ck.CardID);
            modelBuilder.Entity<CardKeyword>()
                .HasOne(ck => ck.Keyword)
                .WithMany(k => k.Cards)
                .HasForeignKey(ck => ck.KeywordID);

            modelBuilder.Entity<CardSubtype>()
                .HasKey(ct => new { ct.CardID, ct.SubtypeID });
            modelBuilder.Entity<CardSubtype>()
                .HasOne(ct => ct.Card)
                .WithMany(c => c.Subtypes)
                .HasForeignKey(ck => ck.CardID);
            modelBuilder.Entity<CardSubtype>()
                .HasOne(ct => ct.Subtype)
                .WithMany(c => c.Cards)
                .HasForeignKey(ct => ct.SubtypeID);

            modelBuilder.Entity<CardSupertype>()
                .HasKey(ct => new { ct.CardID, ct.SupertypeID });
            modelBuilder.Entity<CardSupertype>()
                .HasOne(ct => ct.Card)
                .WithMany(c => c.Supertypes)
                .HasForeignKey(ck => ck.CardID);
            modelBuilder.Entity<CardSupertype>()
                .HasOne(ct => ct.Supertype)
                .WithMany(st => st.Cards)
                .HasForeignKey(ct => ct.SupertypeID);

            modelBuilder.Entity<CardTypeSubtype>()
                .HasKey(ct => new { ct.CardTypeID, ct.SubtypeID });
            modelBuilder.Entity<CardTypeSubtype>()
                .HasOne(ct => ct.CardType)
                .WithMany(c => c.Subtypes)
                .HasForeignKey(ck => ck.CardTypeID);
            modelBuilder.Entity<CardTypeSubtype>()
                .HasOne(ct => ct.Subtype)
                .WithMany(t => t.Types)
                .HasForeignKey(ct => ct.SubtypeID);

            modelBuilder.Entity<CardTypeSupertype>()
                .HasKey(ct => new { ct.CardTypeID, ct.SupertypeID });
            modelBuilder.Entity<CardTypeSupertype>()
                .HasOne(ct => ct.CardType)
                .WithMany(c => c.Supertypes)
                .HasForeignKey(ck => ck.CardTypeID);
            modelBuilder.Entity<CardTypeSupertype>()
                .HasOne(ct => ct.Supertype)
                .WithMany(t => t.Types)
                .HasForeignKey(ct => ct.SupertypeID);
        }
    }
}