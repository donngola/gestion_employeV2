CREATE TABLE [Adresses] (
    [Id] uniqueidentifier NOT NULL,
    [Rue] nvarchar(100) NOT NULL,
    [Ville] nvarchar(40) NOT NULL,
    [CodePostal] varchar(20) NOT NULL,
    [Pays] nvarchar(40) NOT NULL,
    [Region] nvarchar(40) NULL,
    [Tel] varchar(20) NULL,
    CONSTRAINT [PK_Adresses] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Categories] (
    [Id] uniqueidentifier NOT NULL,
    [Nom] nvarchar(40) NOT NULL,
    [Description] nvarchar(1000) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Livreurs] (
    [Id] int NOT NULL IDENTITY,
    [NomSociete] nvarchar(40) NOT NULL,
    [Telephone] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Livreurs] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Regions] (
    [Id] int NOT NULL,
    [Nom] nvarchar(40) NOT NULL,
    CONSTRAINT [PK_Regions] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Clients] (
    [Id] varchar(20) NOT NULL,
    [IdAdresse] uniqueidentifier NOT NULL,
    [NomSociete] nvarchar(100) NOT NULL,
    [NomContact] nvarchar(100) NULL,
    [FonctionContact] nvarchar(100) NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Clients_Adresses_IdAdresse] FOREIGN KEY ([IdAdresse]) REFERENCES [Adresses] ([Id])
);
GO


CREATE TABLE [Employes] (
    [Id] int NOT NULL IDENTITY,
    [IdAdresse] uniqueidentifier NOT NULL,
    [IdManager] int NULL,
    [Nom] nvarchar(40) NOT NULL,
    [Prenom] nvarchar(40) NOT NULL,
    [Fonction] nvarchar(40) NULL,
    [Civilite] nvarchar(40) NULL,
    [DateNaissance] datetime2 NULL,
    [DateEmbauche] datetime2 NULL,
    [Photo] image NULL,
    [Notes] nvarchar(1000) NULL,
    CONSTRAINT [PK_Employes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employes_Adresses_IdAdresse] FOREIGN KEY ([IdAdresse]) REFERENCES [Adresses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Employes_Employes_IdManager] FOREIGN KEY ([IdManager]) REFERENCES [Employes] ([Id])
);
GO


CREATE TABLE [Fournisseurs] (
    [Id] int NOT NULL IDENTITY,
    [IdAdresse] uniqueidentifier NOT NULL,
    [NomSociete] nvarchar(100) NOT NULL,
    [NomContact] nvarchar(100) NULL,
    [FonctionContact] nvarchar(100) NULL,
    [UrlSiteWeb] nvarchar(100) NULL,
    CONSTRAINT [PK_Fournisseurs] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Fournisseurs_Adresses_IdAdresse] FOREIGN KEY ([IdAdresse]) REFERENCES [Adresses] ([Id])
);
GO


CREATE TABLE [Territoires] (
    [Id] varchar(20) NOT NULL,
    [IdRegion] int NOT NULL,
    [Nom] nvarchar(40) NOT NULL,
    CONSTRAINT [PK_Territoires] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Territoires_Regions_IdRegion] FOREIGN KEY ([IdRegion]) REFERENCES [Regions] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [Commandes] (
    [Id] int NOT NULL IDENTITY,
    [IdAdresse] uniqueidentifier NOT NULL,
    [IdClient] varchar(20) NOT NULL,
    [IdEmploye] int NOT NULL,
    [IdLivreur] int NOT NULL,
    [DateCommande] datetime2 NOT NULL,
    [DateLivMaxi] datetime2 NULL,
    [DateLivraison] datetime2 NULL,
    [FraisLivraison] decimal(6,2) NULL,
    CONSTRAINT [PK_Commandes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Commandes_Adresses_IdAdresse] FOREIGN KEY ([IdAdresse]) REFERENCES [Adresses] ([Id]),
    CONSTRAINT [FK_Commandes_Clients_IdClient] FOREIGN KEY ([IdClient]) REFERENCES [Clients] ([Id]),
    CONSTRAINT [FK_Commandes_Employes_IdEmploye] FOREIGN KEY ([IdEmploye]) REFERENCES [Employes] ([Id]),
    CONSTRAINT [FK_Commandes_Livreurs_IdLivreur] FOREIGN KEY ([IdLivreur]) REFERENCES [Livreurs] ([Id])
);
GO


CREATE TABLE [Produits] (
    [Id] int NOT NULL IDENTITY,
    [IdCategorie] uniqueidentifier NOT NULL,
    [IdFournisseur] int NOT NULL,
    [Nom] nvarchar(40) NOT NULL,
    [PU] decimal(8,2) NOT NULL,
    [UnitesEnStock] smallint NOT NULL,
    [NiveauReappro] smallint NOT NULL,
    [Arrete] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_Produits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Produits_Categories_IdCategorie] FOREIGN KEY ([IdCategorie]) REFERENCES [Categories] ([Id]),
    CONSTRAINT [FK_Produits_Fournisseurs_IdFournisseur] FOREIGN KEY ([IdFournisseur]) REFERENCES [Fournisseurs] ([Id])
);
GO


CREATE TABLE [Affectations] (
    [IdEmploye] int NOT NULL,
    [IdTerritoire] varchar(20) NOT NULL,
    CONSTRAINT [PK_Affectations] PRIMARY KEY ([IdEmploye], [IdTerritoire]),
    CONSTRAINT [FK_Affectations_Employes_IdEmploye] FOREIGN KEY ([IdEmploye]) REFERENCES [Employes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Affectations_Territoires_IdTerritoire] FOREIGN KEY ([IdTerritoire]) REFERENCES [Territoires] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [LignesCommandes] (
    [IdCommande] int NOT NULL,
    [IdProduit] int NOT NULL,
    [PU] decimal(8,2) NOT NULL,
    [Quantite] smallint NOT NULL,
    [TauxReduc] real NOT NULL DEFAULT CAST(0 AS real),
    CONSTRAINT [PK_LignesCommandes] PRIMARY KEY ([IdCommande], [IdProduit]),
    CONSTRAINT [FK_LignesCommandes_Commandes_IdCommande] FOREIGN KEY ([IdCommande]) REFERENCES [Commandes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_LignesCommandes_Produits_IdProduit] FOREIGN KEY ([IdProduit]) REFERENCES [Produits] ([Id])
);
GO


CREATE INDEX [IX_Affectations_IdTerritoire] ON [Affectations] ([IdTerritoire]);
GO


CREATE INDEX [IX_Clients_IdAdresse] ON [Clients] ([IdAdresse]);
GO


CREATE INDEX [IX_Commandes_IdAdresse] ON [Commandes] ([IdAdresse]);
GO


CREATE INDEX [IX_Commandes_IdClient] ON [Commandes] ([IdClient]);
GO


CREATE INDEX [IX_Commandes_IdEmploye] ON [Commandes] ([IdEmploye]);
GO


CREATE INDEX [IX_Commandes_IdLivreur] ON [Commandes] ([IdLivreur]);
GO


CREATE UNIQUE INDEX [IX_Employes_IdAdresse] ON [Employes] ([IdAdresse]);
GO


CREATE INDEX [IX_Employes_IdManager] ON [Employes] ([IdManager]);
GO


CREATE INDEX [IX_Fournisseurs_IdAdresse] ON [Fournisseurs] ([IdAdresse]);
GO


CREATE INDEX [IX_LignesCommandes_IdProduit] ON [LignesCommandes] ([IdProduit]);
GO


CREATE INDEX [IX_Produits_IdCategorie] ON [Produits] ([IdCategorie]);
GO


CREATE INDEX [IX_Produits_IdFournisseur] ON [Produits] ([IdFournisseur]);
GO


CREATE INDEX [IX_Territoires_IdRegion] ON [Territoires] ([IdRegion]);
GO


