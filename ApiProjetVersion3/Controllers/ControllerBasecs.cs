using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiProjetVersion3.Data;
using System.Net;

namespace ApiProjetVersion3.Controllers
{
    /*
     centralisation de la gestion d'erreur ajoute une methode d'extension sur le controleur
    Renvoie une reponse HTTP personnalisee pour les erreurs
     
     */
    public static class ControllerBasecs
    {

        // Renvoie une réponse HTTP personnalisée pour les erreurs
        public static ActionResult CustomResponseForError(this ControllerBase controller, Exception ex)
        {


            if(ex is DbUpdateConcurrencyException)
            {
                return controller.Problem("L'entité ou au moins l'une de ses entités filles n'existe pas en base.",
                    null, (int)HttpStatusCode.NotFound, "Aucune modification enregistrée en base.");
            }else if (ex is Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                // Traduit une DbUpdateException en un objet de type ProblemDetails
                //en récupérant l’erreur d’origine via sa propriété InnerException.
                ProblemDetails pb = e.ConvertToProblemDetails();
                return controller.Problem(pb.Detail, null, pb.Status, pb.Title);
            } /*else if (e is ArgumentException)
            {
                return controller.BadRequest(ex.Message);
            }*/
            else throw ex;
        }
    }
}
