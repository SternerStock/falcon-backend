
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/22/2019 19:38:07
-- Generated from EDMX file: D:\Stuff\Code\Repos\falcon-backend\Falcon.MtG\MTGDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MtG-Dev];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CardColor_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardColor] DROP CONSTRAINT [FK_CardColor_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardColor_Color]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardColor] DROP CONSTRAINT [FK_CardColor_Color];
GO
IF OBJECT_ID(N'[dbo].[FK_CardColorIdentity_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardColorIdentity] DROP CONSTRAINT [FK_CardColorIdentity_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardColorIdentity_Color]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardColorIdentity] DROP CONSTRAINT [FK_CardColorIdentity_Color];
GO
IF OBJECT_ID(N'[dbo].[FK_CardSupertype_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardSupertype] DROP CONSTRAINT [FK_CardSupertype_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardSupertype_Supertype]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardSupertype] DROP CONSTRAINT [FK_CardSupertype_Supertype];
GO
IF OBJECT_ID(N'[dbo].[FK_CardCardType_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardCardType] DROP CONSTRAINT [FK_CardCardType_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardCardType_Type]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardCardType] DROP CONSTRAINT [FK_CardCardType_Type];
GO
IF OBJECT_ID(N'[dbo].[FK_CardSubtype_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardSubtype] DROP CONSTRAINT [FK_CardSubtype_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardSubtype_Subtype]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardSubtype] DROP CONSTRAINT [FK_CardSubtype_Subtype];
GO
IF OBJECT_ID(N'[dbo].[FK_TypeSubtype_Type]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TypeSubtype] DROP CONSTRAINT [FK_TypeSubtype_Type];
GO
IF OBJECT_ID(N'[dbo].[FK_TypeSubtype_Subtype]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TypeSubtype] DROP CONSTRAINT [FK_TypeSubtype_Subtype];
GO
IF OBJECT_ID(N'[dbo].[FK_TypeSupertype_Type]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TypeSupertype] DROP CONSTRAINT [FK_TypeSupertype_Type];
GO
IF OBJECT_ID(N'[dbo].[FK_TypeSupertype_Supertype]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TypeSupertype] DROP CONSTRAINT [FK_TypeSupertype_Supertype];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingArtist]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Printings] DROP CONSTRAINT [FK_PrintingArtist];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingWatermark]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Printings] DROP CONSTRAINT [FK_PrintingWatermark];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingFrame]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Printings] DROP CONSTRAINT [FK_PrintingFrame];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingRarity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Printings] DROP CONSTRAINT [FK_PrintingRarity];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingBorder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Printings] DROP CONSTRAINT [FK_PrintingBorder];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Printings] DROP CONSTRAINT [FK_PrintingSet];
GO
IF OBJECT_ID(N'[dbo].[FK_CardPrinting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Printings] DROP CONSTRAINT [FK_CardPrinting];
GO
IF OBJECT_ID(N'[dbo].[FK_CardKeyword_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardKeyword] DROP CONSTRAINT [FK_CardKeyword_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardKeyword_Keyword]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardKeyword] DROP CONSTRAINT [FK_CardKeyword_Keyword];
GO
IF OBJECT_ID(N'[dbo].[FK_CardLayout]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cards] DROP CONSTRAINT [FK_CardLayout];
GO
IF OBJECT_ID(N'[dbo].[FK_CardCard]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cards] DROP CONSTRAINT [FK_CardCard];
GO
IF OBJECT_ID(N'[dbo].[FK_SetSetType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sets] DROP CONSTRAINT [FK_SetSetType];
GO
IF OBJECT_ID(N'[dbo].[FK_PrintingPrice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Pricings] DROP CONSTRAINT [FK_PrintingPrice];
GO
IF OBJECT_ID(N'[dbo].[FK_BlockSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sets] DROP CONSTRAINT [FK_BlockSet];
GO
IF OBJECT_ID(N'[dbo].[FK_CardLegality]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Legalities] DROP CONSTRAINT [FK_CardLegality];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Cards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cards];
GO
IF OBJECT_ID(N'[dbo].[Colors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Colors];
GO
IF OBJECT_ID(N'[dbo].[Supertypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Supertypes];
GO
IF OBJECT_ID(N'[dbo].[Subtypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subtypes];
GO
IF OBJECT_ID(N'[dbo].[CardTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardTypes];
GO
IF OBJECT_ID(N'[dbo].[Sets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sets];
GO
IF OBJECT_ID(N'[dbo].[Keywords]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Keywords];
GO
IF OBJECT_ID(N'[dbo].[Legalities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Legalities];
GO
IF OBJECT_ID(N'[dbo].[Printings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Printings];
GO
IF OBJECT_ID(N'[dbo].[Rarities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rarities];
GO
IF OBJECT_ID(N'[dbo].[ExceptionLogEntries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExceptionLogEntries];
GO
IF OBJECT_ID(N'[dbo].[Layouts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Layouts];
GO
IF OBJECT_ID(N'[dbo].[Artists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Artists];
GO
IF OBJECT_ID(N'[dbo].[Watermarks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Watermarks];
GO
IF OBJECT_ID(N'[dbo].[Frames]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Frames];
GO
IF OBJECT_ID(N'[dbo].[Borders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Borders];
GO
IF OBJECT_ID(N'[dbo].[SetTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SetTypes];
GO
IF OBJECT_ID(N'[dbo].[Pricings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pricings];
GO
IF OBJECT_ID(N'[dbo].[Blocks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Blocks];
GO
IF OBJECT_ID(N'[dbo].[CardColor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardColor];
GO
IF OBJECT_ID(N'[dbo].[CardColorIdentity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardColorIdentity];
GO
IF OBJECT_ID(N'[dbo].[CardSupertype]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardSupertype];
GO
IF OBJECT_ID(N'[dbo].[CardCardType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardCardType];
GO
IF OBJECT_ID(N'[dbo].[CardSubtype]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardSubtype];
GO
IF OBJECT_ID(N'[dbo].[TypeSubtype]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TypeSubtype];
GO
IF OBJECT_ID(N'[dbo].[TypeSupertype]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TypeSupertype];
GO
IF OBJECT_ID(N'[dbo].[CardKeyword]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardKeyword];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cards'
CREATE TABLE [dbo].[Cards] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [ManaCost] nvarchar(50)  NULL,
    [CMC] float  NOT NULL,
    [TypeLine] nvarchar(200)  NOT NULL,
    [OracleText] nvarchar(max)  NOT NULL,
    [Power] nvarchar(max)  NULL,
    [Toughness] nvarchar(max)  NULL,
    [Loyalty] nvarchar(max)  NULL,
    [LayoutID] int  NOT NULL,
    [MainSideID] int  NULL,
    [Side] nvarchar(max)  NULL,
    [EDHRECRank] int  NULL
);
GO

-- Creating table 'Colors'
CREATE TABLE [dbo].[Colors] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Symbol] nvarchar(1)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [BasicLandName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Supertypes'
CREATE TABLE [dbo].[Supertypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Subtypes'
CREATE TABLE [dbo].[Subtypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'CardTypes'
CREATE TABLE [dbo].[CardTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Sets'
CREATE TABLE [dbo].[Sets] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(20)  NOT NULL,
    [KeyruneCode] nvarchar(max)  NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Date] datetime  NOT NULL,
    [SetTypeID] int  NULL,
    [BlockID] int  NULL
);
GO

-- Creating table 'Keywords'
CREATE TABLE [dbo].[Keywords] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Type] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Legalities'
CREATE TABLE [dbo].[Legalities] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Format] nvarchar(max)  NOT NULL,
    [LegalAsCommander] bit  NOT NULL,
    [Legal] bit  NOT NULL,
    [CardID] int  NOT NULL
);
GO

-- Creating table 'Printings'
CREATE TABLE [dbo].[Printings] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [MultiverseId] int  NOT NULL,
    [FlavorText] nvarchar(max)  NULL,
    [CollectorNumber] nvarchar(max)  NULL,
    [Side] nvarchar(max)  NULL,
    [ArtistID] int  NULL,
    [WatermarkID] int  NULL,
    [FrameID] int  NOT NULL,
    [RarityID] int  NOT NULL,
    [BorderID] int  NOT NULL,
    [SetID] int  NOT NULL,
    [CardID] int  NOT NULL
);
GO

-- Creating table 'Rarities'
CREATE TABLE [dbo].[Rarities] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ExceptionLogEntries'
CREATE TABLE [dbo].[ExceptionLogEntries] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RequestJson] nvarchar(max)  NOT NULL,
    [Outcome] nvarchar(200)  NOT NULL,
    [Exception] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Layouts'
CREATE TABLE [dbo].[Layouts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Artists'
CREATE TABLE [dbo].[Artists] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Watermarks'
CREATE TABLE [dbo].[Watermarks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Frames'
CREATE TABLE [dbo].[Frames] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Borders'
CREATE TABLE [dbo].[Borders] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SetTypes'
CREATE TABLE [dbo].[SetTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Pricings'
CREATE TABLE [dbo].[Pricings] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [PrintingID] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Price] float  NOT NULL,
    [Foil] bit  NOT NULL
);
GO

-- Creating table 'Blocks'
CREATE TABLE [dbo].[Blocks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CardColor'
CREATE TABLE [dbo].[CardColor] (
    [CardColor_Color_ID] int  NOT NULL,
    [Colors_ID] int  NOT NULL
);
GO

-- Creating table 'CardColorIdentity'
CREATE TABLE [dbo].[CardColorIdentity] (
    [CardColorIdentity_Color_ID] int  NOT NULL,
    [ColorIdentity_ID] int  NOT NULL
);
GO

-- Creating table 'CardSupertype'
CREATE TABLE [dbo].[CardSupertype] (
    [Cards_ID] int  NOT NULL,
    [Supertypes_ID] int  NOT NULL
);
GO

-- Creating table 'CardCardType'
CREATE TABLE [dbo].[CardCardType] (
    [Cards_ID] int  NOT NULL,
    [Types_ID] int  NOT NULL
);
GO

-- Creating table 'CardSubtype'
CREATE TABLE [dbo].[CardSubtype] (
    [Cards_ID] int  NOT NULL,
    [Subtypes_ID] int  NOT NULL
);
GO

-- Creating table 'TypeSubtype'
CREATE TABLE [dbo].[TypeSubtype] (
    [Types_ID] int  NOT NULL,
    [Subtypes_ID] int  NOT NULL
);
GO

-- Creating table 'TypeSupertype'
CREATE TABLE [dbo].[TypeSupertype] (
    [Types_ID] int  NOT NULL,
    [Supertypes_ID] int  NOT NULL
);
GO

-- Creating table 'CardKeyword'
CREATE TABLE [dbo].[CardKeyword] (
    [Cards_ID] int  NOT NULL,
    [Keywords_ID] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Cards'
ALTER TABLE [dbo].[Cards]
ADD CONSTRAINT [PK_Cards]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Colors'
ALTER TABLE [dbo].[Colors]
ADD CONSTRAINT [PK_Colors]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Supertypes'
ALTER TABLE [dbo].[Supertypes]
ADD CONSTRAINT [PK_Supertypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Subtypes'
ALTER TABLE [dbo].[Subtypes]
ADD CONSTRAINT [PK_Subtypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CardTypes'
ALTER TABLE [dbo].[CardTypes]
ADD CONSTRAINT [PK_CardTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Sets'
ALTER TABLE [dbo].[Sets]
ADD CONSTRAINT [PK_Sets]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Keywords'
ALTER TABLE [dbo].[Keywords]
ADD CONSTRAINT [PK_Keywords]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Legalities'
ALTER TABLE [dbo].[Legalities]
ADD CONSTRAINT [PK_Legalities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Printings'
ALTER TABLE [dbo].[Printings]
ADD CONSTRAINT [PK_Printings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Rarities'
ALTER TABLE [dbo].[Rarities]
ADD CONSTRAINT [PK_Rarities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ExceptionLogEntries'
ALTER TABLE [dbo].[ExceptionLogEntries]
ADD CONSTRAINT [PK_ExceptionLogEntries]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Layouts'
ALTER TABLE [dbo].[Layouts]
ADD CONSTRAINT [PK_Layouts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Artists'
ALTER TABLE [dbo].[Artists]
ADD CONSTRAINT [PK_Artists]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Watermarks'
ALTER TABLE [dbo].[Watermarks]
ADD CONSTRAINT [PK_Watermarks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Frames'
ALTER TABLE [dbo].[Frames]
ADD CONSTRAINT [PK_Frames]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Borders'
ALTER TABLE [dbo].[Borders]
ADD CONSTRAINT [PK_Borders]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SetTypes'
ALTER TABLE [dbo].[SetTypes]
ADD CONSTRAINT [PK_SetTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Pricings'
ALTER TABLE [dbo].[Pricings]
ADD CONSTRAINT [PK_Pricings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Blocks'
ALTER TABLE [dbo].[Blocks]
ADD CONSTRAINT [PK_Blocks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [CardColor_Color_ID], [Colors_ID] in table 'CardColor'
ALTER TABLE [dbo].[CardColor]
ADD CONSTRAINT [PK_CardColor]
    PRIMARY KEY CLUSTERED ([CardColor_Color_ID], [Colors_ID] ASC);
GO

-- Creating primary key on [CardColorIdentity_Color_ID], [ColorIdentity_ID] in table 'CardColorIdentity'
ALTER TABLE [dbo].[CardColorIdentity]
ADD CONSTRAINT [PK_CardColorIdentity]
    PRIMARY KEY CLUSTERED ([CardColorIdentity_Color_ID], [ColorIdentity_ID] ASC);
GO

-- Creating primary key on [Cards_ID], [Supertypes_ID] in table 'CardSupertype'
ALTER TABLE [dbo].[CardSupertype]
ADD CONSTRAINT [PK_CardSupertype]
    PRIMARY KEY CLUSTERED ([Cards_ID], [Supertypes_ID] ASC);
GO

-- Creating primary key on [Cards_ID], [Types_ID] in table 'CardCardType'
ALTER TABLE [dbo].[CardCardType]
ADD CONSTRAINT [PK_CardCardType]
    PRIMARY KEY CLUSTERED ([Cards_ID], [Types_ID] ASC);
GO

-- Creating primary key on [Cards_ID], [Subtypes_ID] in table 'CardSubtype'
ALTER TABLE [dbo].[CardSubtype]
ADD CONSTRAINT [PK_CardSubtype]
    PRIMARY KEY CLUSTERED ([Cards_ID], [Subtypes_ID] ASC);
GO

-- Creating primary key on [Types_ID], [Subtypes_ID] in table 'TypeSubtype'
ALTER TABLE [dbo].[TypeSubtype]
ADD CONSTRAINT [PK_TypeSubtype]
    PRIMARY KEY CLUSTERED ([Types_ID], [Subtypes_ID] ASC);
GO

-- Creating primary key on [Types_ID], [Supertypes_ID] in table 'TypeSupertype'
ALTER TABLE [dbo].[TypeSupertype]
ADD CONSTRAINT [PK_TypeSupertype]
    PRIMARY KEY CLUSTERED ([Types_ID], [Supertypes_ID] ASC);
GO

-- Creating primary key on [Cards_ID], [Keywords_ID] in table 'CardKeyword'
ALTER TABLE [dbo].[CardKeyword]
ADD CONSTRAINT [PK_CardKeyword]
    PRIMARY KEY CLUSTERED ([Cards_ID], [Keywords_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CardColor_Color_ID] in table 'CardColor'
ALTER TABLE [dbo].[CardColor]
ADD CONSTRAINT [FK_CardColor_Card]
    FOREIGN KEY ([CardColor_Color_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Colors_ID] in table 'CardColor'
ALTER TABLE [dbo].[CardColor]
ADD CONSTRAINT [FK_CardColor_Color]
    FOREIGN KEY ([Colors_ID])
    REFERENCES [dbo].[Colors]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardColor_Color'
CREATE INDEX [IX_FK_CardColor_Color]
ON [dbo].[CardColor]
    ([Colors_ID]);
GO

-- Creating foreign key on [CardColorIdentity_Color_ID] in table 'CardColorIdentity'
ALTER TABLE [dbo].[CardColorIdentity]
ADD CONSTRAINT [FK_CardColorIdentity_Card]
    FOREIGN KEY ([CardColorIdentity_Color_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ColorIdentity_ID] in table 'CardColorIdentity'
ALTER TABLE [dbo].[CardColorIdentity]
ADD CONSTRAINT [FK_CardColorIdentity_Color]
    FOREIGN KEY ([ColorIdentity_ID])
    REFERENCES [dbo].[Colors]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardColorIdentity_Color'
CREATE INDEX [IX_FK_CardColorIdentity_Color]
ON [dbo].[CardColorIdentity]
    ([ColorIdentity_ID]);
GO

-- Creating foreign key on [Cards_ID] in table 'CardSupertype'
ALTER TABLE [dbo].[CardSupertype]
ADD CONSTRAINT [FK_CardSupertype_Card]
    FOREIGN KEY ([Cards_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Supertypes_ID] in table 'CardSupertype'
ALTER TABLE [dbo].[CardSupertype]
ADD CONSTRAINT [FK_CardSupertype_Supertype]
    FOREIGN KEY ([Supertypes_ID])
    REFERENCES [dbo].[Supertypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardSupertype_Supertype'
CREATE INDEX [IX_FK_CardSupertype_Supertype]
ON [dbo].[CardSupertype]
    ([Supertypes_ID]);
GO

-- Creating foreign key on [Cards_ID] in table 'CardCardType'
ALTER TABLE [dbo].[CardCardType]
ADD CONSTRAINT [FK_CardCardType_Card]
    FOREIGN KEY ([Cards_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Types_ID] in table 'CardCardType'
ALTER TABLE [dbo].[CardCardType]
ADD CONSTRAINT [FK_CardCardType_Type]
    FOREIGN KEY ([Types_ID])
    REFERENCES [dbo].[CardTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardCardType_Type'
CREATE INDEX [IX_FK_CardCardType_Type]
ON [dbo].[CardCardType]
    ([Types_ID]);
GO

-- Creating foreign key on [Cards_ID] in table 'CardSubtype'
ALTER TABLE [dbo].[CardSubtype]
ADD CONSTRAINT [FK_CardSubtype_Card]
    FOREIGN KEY ([Cards_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Subtypes_ID] in table 'CardSubtype'
ALTER TABLE [dbo].[CardSubtype]
ADD CONSTRAINT [FK_CardSubtype_Subtype]
    FOREIGN KEY ([Subtypes_ID])
    REFERENCES [dbo].[Subtypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardSubtype_Subtype'
CREATE INDEX [IX_FK_CardSubtype_Subtype]
ON [dbo].[CardSubtype]
    ([Subtypes_ID]);
GO

-- Creating foreign key on [Types_ID] in table 'TypeSubtype'
ALTER TABLE [dbo].[TypeSubtype]
ADD CONSTRAINT [FK_TypeSubtype_Type]
    FOREIGN KEY ([Types_ID])
    REFERENCES [dbo].[CardTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Subtypes_ID] in table 'TypeSubtype'
ALTER TABLE [dbo].[TypeSubtype]
ADD CONSTRAINT [FK_TypeSubtype_Subtype]
    FOREIGN KEY ([Subtypes_ID])
    REFERENCES [dbo].[Subtypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TypeSubtype_Subtype'
CREATE INDEX [IX_FK_TypeSubtype_Subtype]
ON [dbo].[TypeSubtype]
    ([Subtypes_ID]);
GO

-- Creating foreign key on [Types_ID] in table 'TypeSupertype'
ALTER TABLE [dbo].[TypeSupertype]
ADD CONSTRAINT [FK_TypeSupertype_Type]
    FOREIGN KEY ([Types_ID])
    REFERENCES [dbo].[CardTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Supertypes_ID] in table 'TypeSupertype'
ALTER TABLE [dbo].[TypeSupertype]
ADD CONSTRAINT [FK_TypeSupertype_Supertype]
    FOREIGN KEY ([Supertypes_ID])
    REFERENCES [dbo].[Supertypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TypeSupertype_Supertype'
CREATE INDEX [IX_FK_TypeSupertype_Supertype]
ON [dbo].[TypeSupertype]
    ([Supertypes_ID]);
GO

-- Creating foreign key on [ArtistID] in table 'Printings'
ALTER TABLE [dbo].[Printings]
ADD CONSTRAINT [FK_PrintingArtist]
    FOREIGN KEY ([ArtistID])
    REFERENCES [dbo].[Artists]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingArtist'
CREATE INDEX [IX_FK_PrintingArtist]
ON [dbo].[Printings]
    ([ArtistID]);
GO

-- Creating foreign key on [WatermarkID] in table 'Printings'
ALTER TABLE [dbo].[Printings]
ADD CONSTRAINT [FK_PrintingWatermark]
    FOREIGN KEY ([WatermarkID])
    REFERENCES [dbo].[Watermarks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingWatermark'
CREATE INDEX [IX_FK_PrintingWatermark]
ON [dbo].[Printings]
    ([WatermarkID]);
GO

-- Creating foreign key on [FrameID] in table 'Printings'
ALTER TABLE [dbo].[Printings]
ADD CONSTRAINT [FK_PrintingFrame]
    FOREIGN KEY ([FrameID])
    REFERENCES [dbo].[Frames]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingFrame'
CREATE INDEX [IX_FK_PrintingFrame]
ON [dbo].[Printings]
    ([FrameID]);
GO

-- Creating foreign key on [RarityID] in table 'Printings'
ALTER TABLE [dbo].[Printings]
ADD CONSTRAINT [FK_PrintingRarity]
    FOREIGN KEY ([RarityID])
    REFERENCES [dbo].[Rarities]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingRarity'
CREATE INDEX [IX_FK_PrintingRarity]
ON [dbo].[Printings]
    ([RarityID]);
GO

-- Creating foreign key on [BorderID] in table 'Printings'
ALTER TABLE [dbo].[Printings]
ADD CONSTRAINT [FK_PrintingBorder]
    FOREIGN KEY ([BorderID])
    REFERENCES [dbo].[Borders]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingBorder'
CREATE INDEX [IX_FK_PrintingBorder]
ON [dbo].[Printings]
    ([BorderID]);
GO

-- Creating foreign key on [SetID] in table 'Printings'
ALTER TABLE [dbo].[Printings]
ADD CONSTRAINT [FK_PrintingSet]
    FOREIGN KEY ([SetID])
    REFERENCES [dbo].[Sets]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingSet'
CREATE INDEX [IX_FK_PrintingSet]
ON [dbo].[Printings]
    ([SetID]);
GO

-- Creating foreign key on [CardID] in table 'Printings'
ALTER TABLE [dbo].[Printings]
ADD CONSTRAINT [FK_CardPrinting]
    FOREIGN KEY ([CardID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardPrinting'
CREATE INDEX [IX_FK_CardPrinting]
ON [dbo].[Printings]
    ([CardID]);
GO

-- Creating foreign key on [Cards_ID] in table 'CardKeyword'
ALTER TABLE [dbo].[CardKeyword]
ADD CONSTRAINT [FK_CardKeyword_Card]
    FOREIGN KEY ([Cards_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Keywords_ID] in table 'CardKeyword'
ALTER TABLE [dbo].[CardKeyword]
ADD CONSTRAINT [FK_CardKeyword_Keyword]
    FOREIGN KEY ([Keywords_ID])
    REFERENCES [dbo].[Keywords]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardKeyword_Keyword'
CREATE INDEX [IX_FK_CardKeyword_Keyword]
ON [dbo].[CardKeyword]
    ([Keywords_ID]);
GO

-- Creating foreign key on [LayoutID] in table 'Cards'
ALTER TABLE [dbo].[Cards]
ADD CONSTRAINT [FK_CardLayout]
    FOREIGN KEY ([LayoutID])
    REFERENCES [dbo].[Layouts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardLayout'
CREATE INDEX [IX_FK_CardLayout]
ON [dbo].[Cards]
    ([LayoutID]);
GO

-- Creating foreign key on [MainSideID] in table 'Cards'
ALTER TABLE [dbo].[Cards]
ADD CONSTRAINT [FK_CardCard]
    FOREIGN KEY ([MainSideID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardCard'
CREATE INDEX [IX_FK_CardCard]
ON [dbo].[Cards]
    ([MainSideID]);
GO

-- Creating foreign key on [SetTypeID] in table 'Sets'
ALTER TABLE [dbo].[Sets]
ADD CONSTRAINT [FK_SetSetType]
    FOREIGN KEY ([SetTypeID])
    REFERENCES [dbo].[SetTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SetSetType'
CREATE INDEX [IX_FK_SetSetType]
ON [dbo].[Sets]
    ([SetTypeID]);
GO

-- Creating foreign key on [PrintingID] in table 'Pricings'
ALTER TABLE [dbo].[Pricings]
ADD CONSTRAINT [FK_PrintingPrice]
    FOREIGN KEY ([PrintingID])
    REFERENCES [dbo].[Printings]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrintingPrice'
CREATE INDEX [IX_FK_PrintingPrice]
ON [dbo].[Pricings]
    ([PrintingID]);
GO

-- Creating foreign key on [BlockID] in table 'Sets'
ALTER TABLE [dbo].[Sets]
ADD CONSTRAINT [FK_BlockSet]
    FOREIGN KEY ([BlockID])
    REFERENCES [dbo].[Blocks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BlockSet'
CREATE INDEX [IX_FK_BlockSet]
ON [dbo].[Sets]
    ([BlockID]);
GO

-- Creating foreign key on [CardID] in table 'Legalities'
ALTER TABLE [dbo].[Legalities]
ADD CONSTRAINT [FK_CardLegality]
    FOREIGN KEY ([CardID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardLegality'
CREATE INDEX [IX_FK_CardLegality]
ON [dbo].[Legalities]
    ([CardID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------