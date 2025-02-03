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


CREATE TABLE [Categorie] (
    [Id] uniqueidentifier NOT NULL,
    [Nom] nvarchar(40) NOT NULL,
    [Description] nvarchar(1000) NULL,
    CONSTRAINT [PK_Categorie] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Livreur] (
    [Id] int NOT NULL IDENTITY,
    [NomSociete] nvarchar(40) NOT NULL,
    [Telephone] nvarchar(20) NOT NULL,
    CONSTRAINT [PK_Livreur] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Regions] (
    [Id] int NOT NULL,
    [Nom] nvarchar(40) NOT NULL,
    CONSTRAINT [PK_Regions] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Client] (
    [Id] varchar(20) NOT NULL,
    [IdAdresse] uniqueidentifier NOT NULL,
    [NomSociete] nvarchar(100) NOT NULL,
    [NomContact] nvarchar(100) NULL,
    [FonctionContact] nvarchar(100) NULL,
    CONSTRAINT [PK_Client] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Client_Adresses_IdAdresse] FOREIGN KEY ([IdAdresse]) REFERENCES [Adresses] ([Id])
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


CREATE TABLE [Fournisseur] (
    [Id] int NOT NULL IDENTITY,
    [IdAdresse] uniqueidentifier NOT NULL,
    [NomSociete] nvarchar(100) NOT NULL,
    [NomContact] nvarchar(100) NULL,
    [FonctionContact] nvarchar(100) NULL,
    [UrlSiteWeb] nvarchar(100) NULL,
    CONSTRAINT [PK_Fournisseur] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Fournisseur_Adresses_IdAdresse] FOREIGN KEY ([IdAdresse]) REFERENCES [Adresses] ([Id])
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


CREATE TABLE [Commande] (
    [Id] int NOT NULL IDENTITY,
    [IdAdresse] uniqueidentifier NOT NULL,
    [IdClient] varchar(20) NOT NULL,
    [IdEmploye] int NOT NULL,
    [IdLivreur] int NOT NULL,
    [DateCommande] datetime2 NOT NULL,
    [DateLivMaxi] datetime2 NULL,
    [DateLivraison] datetime2 NULL,
    [FraisLivraison] decimal(6,2) NULL,
    CONSTRAINT [PK_Commande] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Commande_Adresses_IdAdresse] FOREIGN KEY ([IdAdresse]) REFERENCES [Adresses] ([Id]),
    CONSTRAINT [FK_Commande_Client_IdClient] FOREIGN KEY ([IdClient]) REFERENCES [Client] ([Id]),
    CONSTRAINT [FK_Commande_Employes_IdEmploye] FOREIGN KEY ([IdEmploye]) REFERENCES [Employes] ([Id]),
    CONSTRAINT [FK_Commande_Livreur_IdLivreur] FOREIGN KEY ([IdLivreur]) REFERENCES [Livreur] ([Id])
);
GO


CREATE TABLE [Produit] (
    [Id] int NOT NULL IDENTITY,
    [IdCategorie] uniqueidentifier NOT NULL,
    [IdFournisseur] int NOT NULL,
    [Nom] nvarchar(40) NOT NULL,
    [PU] decimal(8,2) NOT NULL,
    [UnitesEnStock] smallint NOT NULL,
    [NiveauReappro] smallint NOT NULL,
    [Arrete] bit NOT NULL DEFAULT CAST(0 AS bit),
    CONSTRAINT [PK_Produit] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Produit_Categorie_IdCategorie] FOREIGN KEY ([IdCategorie]) REFERENCES [Categorie] ([Id]),
    CONSTRAINT [FK_Produit_Fournisseur_IdFournisseur] FOREIGN KEY ([IdFournisseur]) REFERENCES [Fournisseur] ([Id])
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


CREATE TABLE [LigneCommande] (
    [IdCommande] int NOT NULL,
    [IdProduit] int NOT NULL,
    [PU] decimal(8,2) NOT NULL,
    [Quantite] smallint NOT NULL,
    [TauxReduc] real NOT NULL DEFAULT CAST(0 AS real),
    CONSTRAINT [PK_LigneCommande] PRIMARY KEY ([IdCommande], [IdProduit]),
    CONSTRAINT [FK_LigneCommande_Commande_IdCommande] FOREIGN KEY ([IdCommande]) REFERENCES [Commande] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_LigneCommande_Produit_IdProduit] FOREIGN KEY ([IdProduit]) REFERENCES [Produit] ([Id])
);
GO


CREATE INDEX [IX_Affectations_IdTerritoire] ON [Affectations] ([IdTerritoire]);
GO


CREATE INDEX [IX_Client_IdAdresse] ON [Client] ([IdAdresse]);
GO


CREATE INDEX [IX_Commande_IdAdresse] ON [Commande] ([IdAdresse]);
GO


CREATE INDEX [IX_Commande_IdClient] ON [Commande] ([IdClient]);
GO


CREATE INDEX [IX_Commande_IdEmploye] ON [Commande] ([IdEmploye]);
GO


CREATE INDEX [IX_Commande_IdLivreur] ON [Commande] ([IdLivreur]);
GO


CREATE UNIQUE INDEX [IX_Employes_IdAdresse] ON [Employes] ([IdAdresse]);
GO


CREATE INDEX [IX_Employes_IdManager] ON [Employes] ([IdManager]);
GO


CREATE INDEX [IX_Fournisseur_IdAdresse] ON [Fournisseur] ([IdAdresse]);
GO


CREATE INDEX [IX_LigneCommande_IdProduit] ON [LigneCommande] ([IdProduit]);
GO


CREATE INDEX [IX_Produit_IdCategorie] ON [Produit] ([IdCategorie]);
GO


CREATE INDEX [IX_Produit_IdFournisseur] ON [Produit] ([IdFournisseur]);
GO


CREATE INDEX [IX_Territoires_IdRegion] ON [Territoires] ([IdRegion]);
GO


