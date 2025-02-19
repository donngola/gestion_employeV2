﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiProjetVersion3.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreationBdd2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CodePostal = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    Pays = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Tel = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Livreurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomSociete = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livreurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    IdAdresse = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomSociete = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NomContact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FonctionContact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Adresses_IdAdresse",
                        column: x => x.IdAdresse,
                        principalTable: "Adresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Employes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAdresse = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdManager = table.Column<int>(type: "int", nullable: true),
                    Nom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Fonction = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Civilite = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEmbauche = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Photo = table.Column<byte[]>(type: "image", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employes_Adresses_IdAdresse",
                        column: x => x.IdAdresse,
                        principalTable: "Adresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employes_Employes_IdManager",
                        column: x => x.IdManager,
                        principalTable: "Employes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Fournisseurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAdresse = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NomSociete = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NomContact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FonctionContact = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UrlSiteWeb = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fournisseurs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fournisseurs_Adresses_IdAdresse",
                        column: x => x.IdAdresse,
                        principalTable: "Adresses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Territoires",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false),
                    IdRegion = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Territoires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Territoires_Regions_IdRegion",
                        column: x => x.IdRegion,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Commandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdAdresse = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdClient = table.Column<string>(type: "varchar(20)", nullable: false),
                    IdEmploye = table.Column<int>(type: "int", nullable: false),
                    IdLivreur = table.Column<int>(type: "int", nullable: false),
                    DateCommande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateLivMaxi = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateLivraison = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FraisLivraison = table.Column<decimal>(type: "decimal(6,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commandes_Adresses_IdAdresse",
                        column: x => x.IdAdresse,
                        principalTable: "Adresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commandes_Clients_IdClient",
                        column: x => x.IdClient,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commandes_Employes_IdEmploye",
                        column: x => x.IdEmploye,
                        principalTable: "Employes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Commandes_Livreurs_IdLivreur",
                        column: x => x.IdLivreur,
                        principalTable: "Livreurs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Produits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCategorie = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdFournisseur = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PU = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    UnitesEnStock = table.Column<short>(type: "smallint", nullable: false),
                    NiveauReappro = table.Column<short>(type: "smallint", nullable: false),
                    Arrete = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produits_Categories_IdCategorie",
                        column: x => x.IdCategorie,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Produits_Fournisseurs_IdFournisseur",
                        column: x => x.IdFournisseur,
                        principalTable: "Fournisseurs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Affectations",
                columns: table => new
                {
                    IdEmploye = table.Column<int>(type: "int", nullable: false),
                    IdTerritoire = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Affectations", x => new { x.IdEmploye, x.IdTerritoire });
                    table.ForeignKey(
                        name: "FK_Affectations_Employes_IdEmploye",
                        column: x => x.IdEmploye,
                        principalTable: "Employes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Affectations_Territoires_IdTerritoire",
                        column: x => x.IdTerritoire,
                        principalTable: "Territoires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LignesCommandes",
                columns: table => new
                {
                    IdCommande = table.Column<int>(type: "int", nullable: false),
                    IdProduit = table.Column<int>(type: "int", nullable: false),
                    PU = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Quantite = table.Column<short>(type: "smallint", nullable: false),
                    TauxReduc = table.Column<float>(type: "real", nullable: false, defaultValue: 0f)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignesCommandes", x => new { x.IdCommande, x.IdProduit });
                    table.ForeignKey(
                        name: "FK_LignesCommandes_Commandes_IdCommande",
                        column: x => x.IdCommande,
                        principalTable: "Commandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LignesCommandes_Produits_IdProduit",
                        column: x => x.IdProduit,
                        principalTable: "Produits",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Affectations_IdTerritoire",
                table: "Affectations",
                column: "IdTerritoire");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_IdAdresse",
                table: "Clients",
                column: "IdAdresse");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_IdAdresse",
                table: "Commandes",
                column: "IdAdresse");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_IdClient",
                table: "Commandes",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_IdEmploye",
                table: "Commandes",
                column: "IdEmploye");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_IdLivreur",
                table: "Commandes",
                column: "IdLivreur");

            migrationBuilder.CreateIndex(
                name: "IX_Employes_IdAdresse",
                table: "Employes",
                column: "IdAdresse",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employes_IdManager",
                table: "Employes",
                column: "IdManager");

            migrationBuilder.CreateIndex(
                name: "IX_Fournisseurs_IdAdresse",
                table: "Fournisseurs",
                column: "IdAdresse");

            migrationBuilder.CreateIndex(
                name: "IX_LignesCommandes_IdProduit",
                table: "LignesCommandes",
                column: "IdProduit");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_IdCategorie",
                table: "Produits",
                column: "IdCategorie");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_IdFournisseur",
                table: "Produits",
                column: "IdFournisseur");

            migrationBuilder.CreateIndex(
                name: "IX_Territoires_IdRegion",
                table: "Territoires",
                column: "IdRegion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Affectations");

            migrationBuilder.DropTable(
                name: "LignesCommandes");

            migrationBuilder.DropTable(
                name: "Territoires");

            migrationBuilder.DropTable(
                name: "Commandes");

            migrationBuilder.DropTable(
                name: "Produits");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Employes");

            migrationBuilder.DropTable(
                name: "Livreurs");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Fournisseurs");

            migrationBuilder.DropTable(
                name: "Adresses");
        }
    }
}
