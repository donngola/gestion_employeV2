namespace ApiProjetVersion3.Entities
{
    public class DTO
    {
    }

    public class FormEmploye
    {
        public int Id { get; set; }
        public Guid IdAdresse { get; set; }  /*cle etranger*/
        public int? IdManager { get; set; }  /*propriete nullable peut ou ne pas existe*/
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string? Fonction { get; set; }
        public string? Civilite { get; set; }
        public DateTime? DateNaissance { get; set; }
        public DateTime? DateEmbauche { get; set; }
        public virtual Adresse Adressep { get; set; } = null!;


        // Pour récupérer la photo et la biographie sous forme de fichiers
        public IFormFile? Photo { get; set; }
        public IFormFile? Notes { get; set; }
        /*
         Comme l’entité utilisée pour enregistrer les données en base est de type Employe,
        il faut maintenant transférer les données de l’entité DTO dans une entité Employe,
        en transformant les données de type IFormFile en byte[] ou string.*/
    }

}
