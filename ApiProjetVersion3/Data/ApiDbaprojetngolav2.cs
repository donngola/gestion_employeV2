using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ApiProjetVersion3.Entities;

namespace ApiProjetVersion3.Data
{
    public class ApiDbaprojetngolav2 : DbContext
    {

        public ApiDbaprojetngolav2(DbContextOptions<ApiDbaprojetngolav2> options)
            : base(options)  
        {
  
        }

        public virtual DbSet<Adresse> Adresses { get; set; }
        public virtual DbSet<Employe> Employes { get; set; }
        public virtual DbSet<Region> Régions { get; set; }
        public virtual DbSet<Territoire> Territoires { get; set; }

        public virtual DbSet<Affectation> Affectations { get; set; }

        public virtual DbSet<Categorie> Categories { get; set; }
        public virtual DbSet<Produit> Produits { get; set; }
        public virtual DbSet<Fournisseur> Fournisseurs { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Livreur> Livreurs { get; set; }
        public virtual DbSet<Commande> Commandes { get; set; }
        public virtual DbSet<LigneCommande> LignesCommandes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<Adresse>(entity =>
            {
                entity.ToTable("Adresses");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Ville).HasMaxLength(40);
                entity.Property(e => e.Pays).HasMaxLength(40);
                entity.Property(e => e.Tel).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.CodePostal).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Region).HasMaxLength(40);
                entity.Property(e => e.Rue).HasMaxLength(100);
                /*Entity<T> est une méthode de la classe ModelBuilder qui prend en
                 * paramètre un délégué de type Action<T>. C’est pourquoi on lui passe ici une expression lambda.
                 */

                /*
                 * HasKey, ToTable et Property sont des méthodes d’une classe EntityTypeBuilder, qui permet de 
                 * construire des contraintes sur une entité. Ces méthodes prennent également en paramètre des délégués de
                 */
                /*
                 type Action ou Function. Le principe est le même que dans LINQ : on enchaîne les méthodes 
                avec la syntaxe pointée.*/

                /*
                VakueGeneratedNever indique au moteur de base de données de ne pas générer de valeurs automatiquement 
                pour la colonne (notamment pas d’incrémentation automatique).*/
            });

            modelBuilder.Entity<Employe>(entity =>
            {

                /*contrainetes d'integrite*/
                entity.ToTable("Employes");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Prenom).HasMaxLength(40);
                entity.Property(e => e.Nom).HasMaxLength(40);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.Photo).HasColumnType("image");
                entity.Property(e => e.Fonction).HasMaxLength(40);
                entity.Property(e => e.Civilite).HasMaxLength(40);

                /*relation enver elle meme, 1.N ici employe est considere comme manager*/
                entity.HasOne<Employe>().WithMany().HasForeignKey(d => d.IdManager);

                /*relation I.I employe et adresse*/
                entity.HasOne<Adresse>(e => e.Adressep).WithOne()
                      .HasForeignKey<Employe>(d => d.IdAdresse);
                      //.OnDelete(DeleteBehavior.NoAction);

            });

            modelBuilder.Entity<Affectation>(entity =>
            {
                entity.ToTable("Affectations");
                entity.HasKey(e => new { e.IdEmploye, e.IdTerritoire });

                entity.Property(e => e.IdTerritoire).HasMaxLength(20).IsUnicode(false);

                /*relations c'est preferable de la represente dans l'une de classe implique territoire par exemple */
              /*  entity.HasOne<Employe>().WithMany().HasForeignKey(a => a.IdEmploye);
                entity.HasOne<Territoire>().WithMany().HasForeignKey(a => a.IdTerritoire);*/
            });

            modelBuilder.Entity<Region>(entity =>
            {
                /*contraintes d'integrite*/
                entity.ToTable("Regions");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Nom).HasMaxLength(40);

                /*relations*/
               // entity.HasMany<Territoire>().WithOne().HasForeignKey(d => d.IdRegion);

            });

            modelBuilder.Entity<Territoire>(entity =>
            {
                /*contraintes d'integrite*/
                entity.ToTable("Territoires");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.Nom).HasMaxLength(40);
                /*relations il est prefere de definir la relation dans la table fille*/
                entity.HasOne<Region>(t => t.Region).WithMany(r => r.Territoires).HasForeignKey(d => d.IdRegion);

                /*relation de la table affectation impliquant Employe et Territoire*/

                entity.HasMany<Employe>().WithMany(e =>e.Territoires).UsingEntity<Affectation>(
                   l => l.HasOne<Employe>().WithMany().HasForeignKey(a => a.IdEmploye),
                   r => r.HasOne<Territoire>().WithMany().HasForeignKey(a => a.IdTerritoire));
            });




            // produit/////////////////////////////////////////////////////////////////////////
            modelBuilder.Entity<Categorie>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Nom).HasMaxLength(40);
                entity.Property(e => e.Description).HasMaxLength(1000);
            });

            modelBuilder.Entity<Produit>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Nom).HasMaxLength(40);
                entity.Property(e => e.PU).HasColumnType("decimal(8,2)");
                entity.Property(e => e.Arrete).HasDefaultValue(false);

                entity.HasOne(p => p.Catégorie).WithMany()
                        .HasForeignKey(p => p.IdCategorie)
                        .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Fournisseur>().WithMany()
                        .HasForeignKey(p => p.IdFournisseur)
                        .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Fournisseur>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.NomSociete).HasMaxLength(100);
                entity.Property(e => e.NomContact).HasMaxLength(100);
                entity.Property(e => e.FonctionContact).HasMaxLength(100);
                entity.Property(e => e.UrlSiteWeb).HasMaxLength(100);

                entity.HasOne<Adresse>().WithMany()
                        .HasForeignKey(f => f.IdAdresse)
                        .OnDelete(DeleteBehavior.NoAction);
            });

            //commandes/////////////////////////////////////////////////////

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id).HasMaxLength(20).IsUnicode(false);
                entity.Property(e => e.NomSociete).HasMaxLength(100);
                entity.Property(e => e.NomContact).HasMaxLength(100);
                entity.Property(e => e.FonctionContact).HasMaxLength(100);

                entity.HasOne(c => c.Adresse).WithMany()
                        .HasForeignKey(c => c.IdAdresse)
                        .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Livreur>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.NomSociete).HasMaxLength(40);
                entity.Property(e => e.Telephone).HasMaxLength(20);
            });

            modelBuilder.Entity<Commande>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.FraisLivraison).HasColumnType("decimal(6,2)");

                entity.HasOne(c => c.Livreur).WithMany()
                        .HasForeignKey(c => c.IdLivreur)
                        .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Adresse).WithMany()
                        .HasForeignKey(c => c.IdAdresse)
                        .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne<Client>().WithMany(c => c.Commandes)
                        .HasForeignKey(c => c.IdClient)
                        .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Employe).WithMany()
                        .HasForeignKey(c => c.IdEmploye)
                        .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<LigneCommande>(entity =>
            {
                entity.HasKey(e => new { e.IdCommande, e.IdProduit });

                entity.Property(e => e.PU).HasColumnType("decimal(8,2)");
                entity.Property(e => e.TauxReduc).HasDefaultValue(0);

           

                entity.HasOne<Commande>().WithMany(c => c.Lignes)
                        .HasForeignKey(l => l.IdCommande);

                entity.HasOne(lc => lc.Produit).WithMany()
                        .HasForeignKey(l => l.IdProduit)
                        .OnDelete(DeleteBehavior.NoAction);
            });





            // jeu de donnees
            /*
             if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
{
   modelBuilder.Entity<Employe>().HasData(
   new Employe
   {
      Id = 11,
      Nom = "Prégent",
      Prenom = "Eric",
      IdManager = 2,
      Fonction = "Sales Representative",
      Civilite = "Mr.",
      DateNaissance = new DateTime(2000, 5, 20),
      DateEmbauche = new DateTime(2023, 10, 11),
      IdAdresse = new Guid("01fcbc07-b6ba-4f3a-ac69-891e5a41b14e")
   },
   new Employe
   {
      Id = 12,
      Nom = "Rignaut",
      Prenom = "Solène",
      IdManager = 2,
      Fonction = "Sales Representative",
      Civilite = "Mrs.",
      DateNaissance = new DateTime(2000, 5, 20),
      DateEmbauche = new DateTime(2023, 10, 11),
      IdAdresse = new Guid("01fcbc07-b6ba-4f3a-ac69-891e5a41b14e")
   });
}
             
             */
        }

    }
}
