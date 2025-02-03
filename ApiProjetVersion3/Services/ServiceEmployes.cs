using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using ApiProjetVersion3.Data;
using ApiProjetVersion3.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ApiProjetVersion3.Services
{

    public interface IServiceEmployes
    {
        public Task<List<Employe>> GetAllEmployes(string? recherchenom, DateTime? dateembauchemax);
        public Task<Employe?> GetEmployeId(int id);
        Task<Region?> ObtenirRegion(int id);
        Task<Employe> AjouterEmploye(Employe emp);
        Task<int> SupprimerEmploye(int id);
        Task<int> SupprimerEmployeSiExiste(int id);
        Task<int> SupprimerCommande(int idCommande);
        Task<int> SupprimerCommandeV2(int idCommande);
        Task<Commande?> ObtenirCommande(int id);
        Task<Commande?> AjouterCommandeSansLigne(Commande cmd);
        Task<Commande?> AjouterCommandeAvecLigne(Commande cmd);
        Task<LigneCommande> ModifierLigneCommande(int idCommande, LigneCommande ligne);
        Task<int> ModifierCommande(Commande cmd);
        Task<int> ModifierCommandeEtSesLignes(int idCommande, float tauxReduc);
    }

    public class ServiceEmployes : IServiceEmployes
    {

        private readonly ApiDbaprojetngolav2 _contexte;

        public ServiceEmployes(ApiDbaprojetngolav2 contexteNorthwind)
        {
            _contexte = contexteNorthwind;
        }
        public async Task<List<Employe>> GetAllEmployes(string? recherchenom, DateTime? dateembauchemax)
        {
            //requete a la bd 
            //var req = from e in _contexte.Employes select e;

            //requete partielle a la bd, selectione une partie seulement
            var req = from e in _contexte.Employes
                      .Include(a => a.Adressep)
                      .Include(t => t.Territoires)

                          //filtrage par nom ou date
                      where (recherchenom == null || e.Nom.Contains(recherchenom))
                            && (dateembauchemax == null || e.DateEmbauche <= dateembauchemax)
                      select
                      new Employe
                      {
                          Id = e.Id,
                          Civilite = e.Civilite,
                          Nom = e.Nom,
                          Prenom = e.Prenom,
                          Fonction = e.Fonction,
                          DateEmbauche = e.DateEmbauche,
                          Adressep = e.Adressep, /*cette propriete permet d'ajouter
                                                  * l'adresse et ses proprietes
                                                  ajouter grace a .Include(a => a.Adressep)*/
                          Territoires = e.Territoires,

                      };
            //Tri par date d'embauche descendant
            if (dateembauchemax != null)
            {
                req = req.OrderByDescending(e => e.DateEmbauche);
            }
            else
            {
                req = req.OrderBy(e => e.Nom).ThenBy(e => e.Prenom);
            }

            //execution de la requete
            return await req.ToListAsync();
        }

        public async Task<Employe?> GetEmployeId(int id)
        {
            //FindAsync recherche un élément à partir de sa clé primaire. 
            //var employe =  await _contexte.Employes.FindAsync(id);

            var req = from e in _contexte.Employes
                      .Include(e => e.Adressep)
                      .Include(e => e.Territoires)
                      .ThenInclude(t => t.Region)
                      where (e.Id == id)
                      select e;
            //FirstOrDefaultAsync cette methode retourne le premier elt au cas contraire la valeur pardefaut
            return await req.FirstOrDefaultAsync();
        }

        public async Task<Employe> AjouterEmploye(Employe emp)
        {
            //ajoute l'employe dans le DbSet
            _contexte.Employes.Add(emp);
            //Enregistre l'employe dans la bd et affecte son Id
            await _contexte.SaveChangesAsync();
            //renvoie l'employe avec son Id renseigne
            return emp;
        }

        // Supprime seulement l'employe
        public async Task<int> SupprimerEmploye(int id)
        {
            Employe emp = new()
            {
                Id = id,
            };

            //_contexte.Remove(ligne);
            //Remove permet de rattacher l’entité au contexte à l’état Deleted
            //Entry ne rattache au contexte que l’entité elle-même, sans ses filles.
            //mais l'adresse n'est pas supprimee
            _contexte.Entry(emp).State = EntityState.Deleted;
            return await _contexte.SaveChangesAsync();
        }

        // Supprime une ligne de commande en vérifiant tout d'abord qu'elle existe
        public async Task<int> SupprimerEmployeSiExiste(int id)
        {

            //ici on verifie si l'employe existe avant de le supprime en faisant une requete dans la bd
            //mais l'adresse n'est pas supprimee
            Employe? emp = await _contexte.Employes.FindAsync(id);
            if (emp == null) return 0;
            else
                // emp.Adressep = null!;
                // _contexte.Remove(emp);
                _contexte.Entry(emp).State = EntityState.Deleted;
            return await _contexte.SaveChangesAsync();
        }

        //methode pour recuper une region et ses territoires
        public async Task<Region?> ObtenirRegion(int id)
        {
            var req = from e in _contexte.Régions
                      .Include(r => r.Territoires)
                      where (e.Id == id)
                      select e;
            return await req.FirstOrDefaultAsync();

        }

        //permet d'obtenir une commade et ses lignes
        public async Task<Commande?> ObtenirCommande(int id)
        {
            var req = from c in _contexte.Commandes
                      .Include(l => l.Lignes)
                      where (c.Id == id)
                      select c;


            return await req.FirstOrDefaultAsync();
        }

        //Supposons qu’on souhaite créer une commande pour un client,
        //un livreur, une adresse et un employé déjà existants en base,



        //sans ligne de commande
        public async Task<Commande?> AjouterCommandeSansLigne(Commande cmd)
        {
            // On remet les propriétés de navigation correspondantes à null
            //pour qu'ils ne soient pas ajouter dans la bd (vu qu'ils sont declare avec new dans les entites)

            cmd.Adresse = null;
            cmd.Livreur = null;
            cmd.Employe = null;

            _contexte.Commandes.Add(cmd);
            await _contexte.SaveChangesAsync();

            return cmd;
        }

        //avec lignes de commande
        public async Task<Commande?> AjouterCommandeAvecLigne(Commande cmd)
        {
            // On remet les propriétés de navigation correspondantes à null
            cmd.Employe = null!;
            cmd.Adresse = null!;
            cmd.Livreur = null!;
            // cmd.Lignes = null!;
            foreach (var ligne in cmd.Lignes) ligne.Produit = null!;

            _contexte.Commandes.Add(cmd);
            await _contexte.SaveChangesAsync();

            return cmd;
        }

        //modifie une ligne de commande
        public async Task<LigneCommande> ModifierLigneCommande(int idCommande, LigneCommande ligne)
        {
            ligne.IdCommande = idCommande;

            // Rattache l'entité au suivi, sans ses filles, en passant son état à Modified
            EntityEntry<LigneCommande> ent = _contexte.Entry(ligne);
            ent.State = EntityState.Modified;

            // Empêche la modification du prix unitaire de la ligne
            // ent.Property(l => l.PU).IsModified = false;

            await _contexte.SaveChangesAsync();

            return ent.Entity;
        }

        // Modifie une commande avec ses lignes
        public async Task<int> ModifierCommande(Commande cmd)
        {

            // Passe la commande et ses lignes à l'état Modified
            _contexte.Entry(cmd).State = EntityState.Modified;
            foreach (var ligne in cmd.Lignes)
            {
                _contexte.Entry(ligne).State = EntityState.Modified;
            }

            return await _contexte.SaveChangesAsync();
        }

        // Met à jour le taux de réduction sur toutes les lignes d'une commande
        public async Task<int> ModifierCommandeEtSesLignes(int idCommande, float tauxReduc)
        {
            int nbmodifs = await _contexte.LignesCommandes.Where(lc => lc.IdCommande == idCommande)
               .ExecuteUpdateAsync(setter => setter.SetProperty(l => l.TauxReduc, tauxReduc));

            return nbmodifs;
        }
        public async Task<int> SupprimerCommande(int idCommande)
        {
            Commande commande = new() { Id = idCommande };
            _contexte.Entry(commande).State = EntityState.Deleted;
            return await _contexte.SaveChangesAsync();
        }
        // Supprime une commande et ses lignes en chargeant tout d'abord les entités
        public async Task<int> SupprimerCommandeV2(int idCommande)
        {
            // Récupère la commande et ses lignes sans les rattacher au suivi
            var req = from c in _contexte.Commandes.Include(c => c.Lignes)
                      where c.Id == idCommande
                      select c;

            var cmd = await req.FirstOrDefaultAsync();
            if (cmd == null) return 0;

            // Passe la commande et ses lignes à l'état Deleted
            _contexte.Entry(cmd).State = EntityState.Deleted;
            foreach (var ligne in cmd.Lignes)
            {
                _contexte.Entry(ligne).State = EntityState.Deleted;
            }

            return await _contexte.SaveChangesAsync();
        }









    }
}
