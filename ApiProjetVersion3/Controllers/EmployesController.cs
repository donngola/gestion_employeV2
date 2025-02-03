using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProjetVersion3.Data;
using ApiProjetVersion3.Entities;
using ApiProjetVersion3.Services;

namespace ApiProjetVersion3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployesController : ControllerBase
    {
        private readonly IServiceEmployes _iservice_employe;
        // activer la journalisation des infos sensible
        private readonly ILogger<EmployesController> _logger;

        public EmployesController(IServiceEmployes icontext, ILogger<EmployesController> logger)
        {
            _iservice_employe = icontext;
            _logger = logger;
        }


        //Nswag va lire le contenu pour completer la doc de l'api (commentaire triple slash)
        /// <summary>
        /// Recupere la liste d'employes
        /// </summary>
        /// <param name="recherchenom">Recherche l'employe par nom</param>
        /// <param name="dateembauche">Recherche l'employe par date d'embauche </param>
        /// <returns></returns>
        // GET: api/Employes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employe>>> 
            GetEmployes([FromQuery] string? recherchenom, [FromQuery] DateTime? dateembauche )
        {

            var employes = await _iservice_employe.GetAllEmployes(recherchenom, dateembauche);
            return Ok(employes);
        }

        /// <summary>
        /// Recupere l'employe par son ID
        /// </summary>
        /// <param name="id">Identifiant de l'employe</param>
        /// <returns></returns>
        // GET: api/Employes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employe>> GetEmploye(int id)
        {
            var employe = await _iservice_employe.GetEmployeId(id);
            employe.Photo = null;

            if (employe == null)
            {
                return NotFound();
            }

            return Ok(employe);
        }

        /// <summary>
        /// Ajoute un enploye dans la bdd
        /// </summary>
        /// <param name="employe">Employe</param>
        /// <returns></returns>

        // POST: api/Employes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employe>> PostEmploye(Employe employe)
        {

            try
            {
                //Enregistre l'employe dans la bd et le recupere avec son Id genere automatiquement
                Employe res = await _iservice_employe.AjouterEmploye(employe);


                // Renvoie une réponse de code 201 avec l'en-tête
                // "location: <url d'accès à l’employé>" et un corps contenant l’employé
                return CreatedAtAction(nameof(GetEmploye), new { id = res.Id }, res);
            }
            catch (DbUpdateException e)
            {

                //cette classe grace a la methode Probleme permet de Renvoie une reponse HTTP personnalisee pour les erreurs
                ProblemDetails pb = e.ConvertToProblemDetails();
                return Problem(pb.Detail, null, pb.Status, pb.Title);
            }
        }

        /// <summary>
        /// Ajoute un employe formulaire
        /// </summary>
        /// <param name="fe"></param>
        /// <returns></returns>
        // POST: api/Employes/formdata
        [HttpPost("formdata")]
        public async Task<ActionResult<Employe>> PostEmployéFormData([FromForm] FormEmploye fe)
        {

            try
            {
                Employe emp = new()
                {
                    IdAdresse = fe.IdAdresse,
                    IdManager = fe.IdManager,
                    Nom = fe.Nom,
                    Prenom = fe.Prenom,
                    Fonction = fe.Fonction,
                    Civilite = fe.Civilite,
                    DateNaissance = fe.DateNaissance,
                    DateEmbauche = fe.DateEmbauche,
                    Adressep = fe.Adressep,
                };

                // Récupère les données de l'adresse

                emp.Adressep = new()
                {
                    Id = fe.Adressep.Id,
                    Rue = fe.Adressep.Rue,
                    CodePostal = fe.Adressep.CodePostal,
                    Ville = fe.Adressep.Ville,
                    Region = fe.Adressep.Region,
                    Pays = fe.Adressep.Pays,
                    Tel = fe.Adressep.Tel
                };

                // Récupère les données du fichier photo
                if (fe.Photo != null)
                {
                    using Stream stream = fe.Photo.OpenReadStream();
                    emp.Photo = new byte[fe.Photo.Length];
                    await stream.ReadAsync(emp.Photo);
                }

                // Récupère les données du fichier notes
                if (fe.Notes != null)
                {
                    using StreamReader reader = new(fe.Notes.OpenReadStream());
                    emp.Notes = await reader.ReadToEndAsync();
                }

                Employe res = await _iservice_employe.AjouterEmploye(emp);


                // Renvoie une réponse de code 201 avec l'en-tête
                // "location: <url d'accès à l’employé>" et un corps contenant l’employé
                // return CreatedAtAction(nameof(GetEmploye), new { id = emp.Id }, res) ;
                string uri = Url.Action(nameof(GetEmploye), new { id = emp.Id }) ?? "";
                return Created(uri, res);



            }
            catch (Exception e)
            {
                // ProblemDetails pb = e.ConvertToProblemDetails();
                //return Problem(pb.Detail, null, pb.Status, pb.Title);
                return this.CustomResponseForError(e);

                // return this.CustomResponseForError(e, emp, _logger);
                //JE surcharge cette methode pour faire la journalisation

            }
        }

        /// <summary>
        /// Supprimme un employe
        /// </summary>
        /// <param name="id">Id employe a supprimer</param>
        /// <returns></returns>
        // DELETE: api/Employes/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> SupprimeEmploye(int id)
        {
            try
            {
                int nbSupprime = await _iservice_employe.SupprimerEmploye(id);
                if (nbSupprime == 0)
                    return NotFound($"l'employe n'a pas d'adresse {id}");
                return NoContent();
            }
            catch (Exception e)
            {
                return this.CustomResponseForError(e);
            }
        }

        /// <summary>
        /// Recupe une region
        /// </summary>
        /// <param name="id">Id region a recuperer</param>
        /// <returns></returns>
        // GET: api/Employes/5
        [HttpGet("/api/Regions/{id}")]
        public async Task<ActionResult<Employe>> Getregion(int id)
        {
            var employe = await _iservice_employe.ObtenirRegion(id);

            if (employe == null)
            {
                return NotFound();
            }

            return Ok(employe);
        }

        [HttpGet("/api/Commandes/{id}")]
        public async Task<ActionResult<Commande>> GetCommande(int id)
        {
            var commande = await _iservice_employe.ObtenirCommande(id);
            if(commande == null)
            {  return NotFound();}
            return Ok(commande);
        }

        // POST: api/Commandes
        [HttpPost("/api/Commandes")]
        public async Task<ActionResult<Commande>> PostCommande(Commande cmd)
        {
            Commande? commande = await _iservice_employe.AjouterCommandeAvecLigne(cmd);

            string uri = Url.Action(nameof(GetCommande), new { id = commande?.Id }) ?? "";
            return Created(uri, commande);
        }

        // PUT: api/Commandes/831/Lignes
        [HttpPut("/{idCommande}/Lignes")]
        public async Task<IActionResult> PutLigneCommande(int idCommande, LigneCommande ligne)
        {
            try
            {
                await _iservice_employe.ModifierLigneCommande(idCommande, ligne);
                return NoContent();
            }
            catch (Exception e)
            {
                return this.CustomResponseForError(e);
            }
        }


        // PUT: api/Commandes  a continuer
        [HttpPut("/api/Commandes")]
        public async Task<IActionResult> PutLigneCommande(Commande cmd)
        {
            try
            {
                await _iservice_employe.ModifierCommande(cmd);
                return NoContent();
            }
            catch (Exception e)
            {
                return this.CustomResponseForError(e);
            }
        }

        [HttpDelete("/api/Commandes/{id}")]
        public async Task<IActionResult> DeleteCommade(int id)
        {
            try
            {
                int nbSupprime = await _iservice_employe.SupprimerCommandeV2(id);
                if (nbSupprime == 0)
                    return NotFound($"Il n'existe aucune ligne pour la commande {id}");
                return NoContent();
            }
            catch (Exception e)
            {
                return this.CustomResponseForError(e);
            }
        }

       
      







        /*
        
                // PUT: api/Employes/5
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPut("{id}")]
                public async Task<IActionResult> PutEmploye(int id, Employe employe)
                {
                    if (id != employe.Id)
                    {
                        return BadRequest();
                    }

                    _context.Entry(employe).State = EntityState.Modified;

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeExists(id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }

                    return NoContent();
                }

                // POST: api/Employes
                // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
                [HttpPost]
                public async Task<ActionResult<Employe>> PostEmploye(Employe employe)
                {
                    _context.Employe.Add(employe);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetEmploye", new { id = employe.Id }, employe);
                }

                // DELETE: api/Employes/5
                [HttpDelete("{id}")]
                public async Task<IActionResult> DeleteEmploye(int id)
                {
                    var employe = await _context.Employe.FindAsync(id);
                    if (employe == null)
                    {
                        return NotFound();
                    }

                    _context.Employe.Remove(employe);
                    await _context.SaveChangesAsync();

                    return NoContent();
                }

                private bool EmployeExists(int id)
                {
                    return _context.Employe.Any(e => e.Id == id);
                }
        */
    }
}
