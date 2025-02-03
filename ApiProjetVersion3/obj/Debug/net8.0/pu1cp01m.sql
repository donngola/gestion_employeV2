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


CREATE TABLE [Regions] (
    [Id] int NOT NULL,
    [Nom] nvarchar(40) NOT NULL,
    CONSTRAINT [PK_Regions] PRIMARY KEY ([Id])
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
    CONSTRAINT [FK_Employes_Adresses_IdAdresse] FOREIGN KEY ([IdAdresse]) REFERENCES [Adresses] ([Id]),
    CONSTRAINT [FK_Employes_Employes_IdManager] FOREIGN KEY ([IdManager]) REFERENCES [Employes] ([Id])
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


CREATE TABLE [Affectations] (
    [IdEmploye] int NOT NULL,
    [IdTerritoire] varchar(20) NOT NULL,
    CONSTRAINT [PK_Affectations] PRIMARY KEY ([IdEmploye], [IdTerritoire]),
    CONSTRAINT [FK_Affectations_Employes_IdEmploye] FOREIGN KEY ([IdEmploye]) REFERENCES [Employes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Affectations_Territoires_IdTerritoire] FOREIGN KEY ([IdTerritoire]) REFERENCES [Territoires] ([Id]) ON DELETE CASCADE
);
GO


CREATE INDEX [IX_Affectations_IdTerritoire] ON [Affectations] ([IdTerritoire]);
GO


CREATE UNIQUE INDEX [IX_Employes_IdAdresse] ON [Employes] ([IdAdresse]);
GO


CREATE INDEX [IX_Employes_IdManager] ON [Employes] ([IdManager]);
GO


CREATE INDEX [IX_Territoires_IdRegion] ON [Territoires] ([IdRegion]);
GO


