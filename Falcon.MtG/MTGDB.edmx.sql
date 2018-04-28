
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/28/2018 12:35:39
-- Generated from EDMX file: D:\Stuff\Code\Repos\falcon-backend\Falcon.MtG\MTGDB.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [MtGLocalDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CardSet_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardSet] DROP CONSTRAINT [FK_CardSet_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardSet_Set]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardSet] DROP CONSTRAINT [FK_CardSet_Set];
GO
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
IF OBJECT_ID(N'[dbo].[FK_CardType_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardType] DROP CONSTRAINT [FK_CardType_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardType_Type]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardType] DROP CONSTRAINT [FK_CardType_Type];
GO
IF OBJECT_ID(N'[dbo].[FK_CardSubtype_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardSubtype] DROP CONSTRAINT [FK_CardSubtype_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardSubtype_Subtype]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardSubtype] DROP CONSTRAINT [FK_CardSubtype_Subtype];
GO
IF OBJECT_ID(N'[dbo].[FK_CardAbility_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardAbility] DROP CONSTRAINT [FK_CardAbility_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardAbility_Ability]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardAbility] DROP CONSTRAINT [FK_CardAbility_Ability];
GO
IF OBJECT_ID(N'[dbo].[FK_CardSides]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cards] DROP CONSTRAINT [FK_CardSides];
GO
IF OBJECT_ID(N'[dbo].[FK_CardRarity_Card]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardRarity] DROP CONSTRAINT [FK_CardRarity_Card];
GO
IF OBJECT_ID(N'[dbo].[FK_CardRarity_Rarity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CardRarity] DROP CONSTRAINT [FK_CardRarity_Rarity];
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
IF OBJECT_ID(N'[dbo].[Types]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Types];
GO
IF OBJECT_ID(N'[dbo].[Sets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sets];
GO
IF OBJECT_ID(N'[dbo].[Abilities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Abilities];
GO
IF OBJECT_ID(N'[dbo].[Rarities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rarities];
GO
IF OBJECT_ID(N'[dbo].[CardSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardSet];
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
IF OBJECT_ID(N'[dbo].[CardType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardType];
GO
IF OBJECT_ID(N'[dbo].[CardSubtype]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardSubtype];
GO
IF OBJECT_ID(N'[dbo].[CardAbility]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardAbility];
GO
IF OBJECT_ID(N'[dbo].[CardRarity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CardRarity];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Cards'
CREATE TABLE [dbo].[Cards] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [MultiverseId] int  NULL,
    [ManaCost] nvarchar(50)  NULL,
    [CMC] int  NULL,
    [TypeLine] nvarchar(200)  NULL,
    [OracleText] nvarchar(max)  NULL,
    [FlavorText] nvarchar(max)  NULL,
    [Power] int  NULL,
    [Toughness] int  NULL,
    [CommanderLegal] bit  NOT NULL,
    [LatestPrintDate] datetime  NOT NULL,
    [IsPrimarySide] bit  NOT NULL,
    [TinyLeadersLegal] bit  NOT NULL,
    [BrawlLegal] bit  NOT NULL,
    [TinyLeadersCmdrLegal] bit  NOT NULL,
    [OtherSide_ID] int  NULL
);
GO

-- Creating table 'Colors'
CREATE TABLE [dbo].[Colors] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Symbol] nvarchar(1)  NOT NULL,
    [Name] nvarchar(50)  NOT NULL
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

-- Creating table 'Types'
CREATE TABLE [dbo].[Types] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Sets'
CREATE TABLE [dbo].[Sets] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(20)  NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Block] nvarchar(200)  NULL,
    [Type] nvarchar(max)  NOT NULL,
    [Border] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Abilities'
CREATE TABLE [dbo].[Abilities] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(200)  NOT NULL,
    [Type] nvarchar(200)  NOT NULL
);
GO

-- Creating table 'Rarities'
CREATE TABLE [dbo].[Rarities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CardSet'
CREATE TABLE [dbo].[CardSet] (
    [CardSet_Set_ID] int  NOT NULL,
    [Sets_ID] int  NOT NULL
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
    [CardSupertype_Supertype_ID] int  NOT NULL,
    [Supertypes_ID] int  NOT NULL
);
GO

-- Creating table 'CardType'
CREATE TABLE [dbo].[CardType] (
    [CardType_Type_ID] int  NOT NULL,
    [Types_ID] int  NOT NULL
);
GO

-- Creating table 'CardSubtype'
CREATE TABLE [dbo].[CardSubtype] (
    [CardSubtype_Subtype_ID] int  NOT NULL,
    [Subtypes_ID] int  NOT NULL
);
GO

-- Creating table 'CardAbility'
CREATE TABLE [dbo].[CardAbility] (
    [CardAbility_Ability_ID] int  NOT NULL,
    [Abilities_ID] int  NOT NULL
);
GO

-- Creating table 'CardRarity'
CREATE TABLE [dbo].[CardRarity] (
    [CardRarity_Rarity_ID] int  NOT NULL,
    [Rarities_Id] int  NOT NULL
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

-- Creating primary key on [ID] in table 'Types'
ALTER TABLE [dbo].[Types]
ADD CONSTRAINT [PK_Types]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Sets'
ALTER TABLE [dbo].[Sets]
ADD CONSTRAINT [PK_Sets]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Abilities'
ALTER TABLE [dbo].[Abilities]
ADD CONSTRAINT [PK_Abilities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Id] in table 'Rarities'
ALTER TABLE [dbo].[Rarities]
ADD CONSTRAINT [PK_Rarities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [CardSet_Set_ID], [Sets_ID] in table 'CardSet'
ALTER TABLE [dbo].[CardSet]
ADD CONSTRAINT [PK_CardSet]
    PRIMARY KEY CLUSTERED ([CardSet_Set_ID], [Sets_ID] ASC);
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

-- Creating primary key on [CardSupertype_Supertype_ID], [Supertypes_ID] in table 'CardSupertype'
ALTER TABLE [dbo].[CardSupertype]
ADD CONSTRAINT [PK_CardSupertype]
    PRIMARY KEY CLUSTERED ([CardSupertype_Supertype_ID], [Supertypes_ID] ASC);
GO

-- Creating primary key on [CardType_Type_ID], [Types_ID] in table 'CardType'
ALTER TABLE [dbo].[CardType]
ADD CONSTRAINT [PK_CardType]
    PRIMARY KEY CLUSTERED ([CardType_Type_ID], [Types_ID] ASC);
GO

-- Creating primary key on [CardSubtype_Subtype_ID], [Subtypes_ID] in table 'CardSubtype'
ALTER TABLE [dbo].[CardSubtype]
ADD CONSTRAINT [PK_CardSubtype]
    PRIMARY KEY CLUSTERED ([CardSubtype_Subtype_ID], [Subtypes_ID] ASC);
GO

-- Creating primary key on [CardAbility_Ability_ID], [Abilities_ID] in table 'CardAbility'
ALTER TABLE [dbo].[CardAbility]
ADD CONSTRAINT [PK_CardAbility]
    PRIMARY KEY CLUSTERED ([CardAbility_Ability_ID], [Abilities_ID] ASC);
GO

-- Creating primary key on [CardRarity_Rarity_ID], [Rarities_Id] in table 'CardRarity'
ALTER TABLE [dbo].[CardRarity]
ADD CONSTRAINT [PK_CardRarity]
    PRIMARY KEY CLUSTERED ([CardRarity_Rarity_ID], [Rarities_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CardSet_Set_ID] in table 'CardSet'
ALTER TABLE [dbo].[CardSet]
ADD CONSTRAINT [FK_CardSet_Card]
    FOREIGN KEY ([CardSet_Set_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Sets_ID] in table 'CardSet'
ALTER TABLE [dbo].[CardSet]
ADD CONSTRAINT [FK_CardSet_Set]
    FOREIGN KEY ([Sets_ID])
    REFERENCES [dbo].[Sets]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardSet_Set'
CREATE INDEX [IX_FK_CardSet_Set]
ON [dbo].[CardSet]
    ([Sets_ID]);
GO

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

-- Creating foreign key on [CardSupertype_Supertype_ID] in table 'CardSupertype'
ALTER TABLE [dbo].[CardSupertype]
ADD CONSTRAINT [FK_CardSupertype_Card]
    FOREIGN KEY ([CardSupertype_Supertype_ID])
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

-- Creating foreign key on [CardType_Type_ID] in table 'CardType'
ALTER TABLE [dbo].[CardType]
ADD CONSTRAINT [FK_CardType_Card]
    FOREIGN KEY ([CardType_Type_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Types_ID] in table 'CardType'
ALTER TABLE [dbo].[CardType]
ADD CONSTRAINT [FK_CardType_Type]
    FOREIGN KEY ([Types_ID])
    REFERENCES [dbo].[Types]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardType_Type'
CREATE INDEX [IX_FK_CardType_Type]
ON [dbo].[CardType]
    ([Types_ID]);
GO

-- Creating foreign key on [CardSubtype_Subtype_ID] in table 'CardSubtype'
ALTER TABLE [dbo].[CardSubtype]
ADD CONSTRAINT [FK_CardSubtype_Card]
    FOREIGN KEY ([CardSubtype_Subtype_ID])
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

-- Creating foreign key on [CardAbility_Ability_ID] in table 'CardAbility'
ALTER TABLE [dbo].[CardAbility]
ADD CONSTRAINT [FK_CardAbility_Card]
    FOREIGN KEY ([CardAbility_Ability_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Abilities_ID] in table 'CardAbility'
ALTER TABLE [dbo].[CardAbility]
ADD CONSTRAINT [FK_CardAbility_Ability]
    FOREIGN KEY ([Abilities_ID])
    REFERENCES [dbo].[Abilities]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardAbility_Ability'
CREATE INDEX [IX_FK_CardAbility_Ability]
ON [dbo].[CardAbility]
    ([Abilities_ID]);
GO

-- Creating foreign key on [OtherSide_ID] in table 'Cards'
ALTER TABLE [dbo].[Cards]
ADD CONSTRAINT [FK_CardSides]
    FOREIGN KEY ([OtherSide_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardSides'
CREATE INDEX [IX_FK_CardSides]
ON [dbo].[Cards]
    ([OtherSide_ID]);
GO

-- Creating foreign key on [CardRarity_Rarity_ID] in table 'CardRarity'
ALTER TABLE [dbo].[CardRarity]
ADD CONSTRAINT [FK_CardRarity_Card]
    FOREIGN KEY ([CardRarity_Rarity_ID])
    REFERENCES [dbo].[Cards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Rarities_Id] in table 'CardRarity'
ALTER TABLE [dbo].[CardRarity]
ADD CONSTRAINT [FK_CardRarity_Rarity]
    FOREIGN KEY ([Rarities_Id])
    REFERENCES [dbo].[Rarities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CardRarity_Rarity'
CREATE INDEX [IX_FK_CardRarity_Rarity]
ON [dbo].[CardRarity]
    ([Rarities_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------