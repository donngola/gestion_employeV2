﻿using System.ComponentModel.DataAnnotations;

namespace ApiProjetVersion3.Entities
{
     public class Adresse
     {
         public Guid Id { get; set; }  //type uniqueidentifier
         public string Rue { get; set; } = string.Empty; //proprite non nulable type reference doit etre initialisee
         public string Ville { get; set; } = string.Empty;
         public string CodePostal { get; set; } = string.Empty;
         public string Pays { get; set; } = string.Empty;
         public string? Region { get; set; }
         public string? Tel { get; set; }
     }

      public class Employe
      {
          public int Id { get; set; }
          public Guid IdAdresse { get; set; }  //cle etranger
          public int? IdManager { get; set; }  //propriete nullable peut ou ne pas existe
          public string Nom { get; set; } = string.Empty;
          public string Prenom { get; set; } = string.Empty;
          public string? Fonction { get; set; }
          public string? Civilite { get; set; }
          public DateTime? DateNaissance { get; set; }
          public DateTime? DateEmbauche { get; set; }
          public byte[]? Photo { get; set; }
          public string? Notes { get; set; }


          public virtual Adresse Adressep { get; set; } = new();
        public virtual List<Territoire> Territoires { get; set; } = new();
      }


    // classe de validation automatic

  /*  public class Adresse
    {
        public Guid Id { get; set; }  type uniqueidentifier
        [Required(ErrorMessage = "La rue doit être renseignée")]
        public string Rue { get; set; } = string.Empty; proprite non nulable type reference doit etre initialisee
        [Required(ErrorMessage = "La ville doit être renseignée")]
        public string Ville { get; set; } = string.Empty;
        [Required(ErrorMessage = "Le code postal doit être renseigné")]
        public string CodePostal { get; set; } = string.Empty;
        [Required(ErrorMessage = "Le pays doit être renseigné")]
        public string Pays { get; set; } = string.Empty;
        public string? Region { get; set; }

        [Phone(ErrorMessage = "Le N° ne doit contenir que des chiffres et éventuellement les caractères suivants : + - . ( ) et espace")]
        public string? Tel { get; set; }
   /* }

    public class Employe
    {
        public int Id { get; set; }
        public Guid IdAdresse { get; set; }  cle etranger
        public int? IdManager { get; set; }  propriete nullable peut ou ne pas existe

        [Required(ErrorMessage = "Le nom doit être renseigné")]
        public string Nom { get; set; } = string.Empty;
        [Required(ErrorMessage = "Le prénom doit être renseignée")]
        public string Prenom { get; set; } = string.Empty;
        public string? Fonction { get; set; }
        public string? Civilite { get; set; }
        public DateTime? DateNaissance { get; set; }
        public DateTime? DateEmbauche { get; set; }
        public byte[]? Photo { get; set; }
        [MaxLength(1000, ErrorMessage = "La biographie ne doit pas dépasser 1000 caractères")]
        public string? Notes { get; set; }


        //dans le cas d'adresse on veut ajouter un emplye et adresse qui n'existe pas dans la bd
        // public virtual Adresse Adressep { get; set; } = null!;

        //dans le cas de commande on veut ajouter une commande dont l'employe et son adresse existe dans la bd
        public virtual Adresse Adressep { get; set; } = new();
        public virtual List<Territoire> Territoires { get; set; } = new();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateEmbauche != null && DateNaissance != null
                && DateEmbauche < DateNaissance.Value.AddYears(18))
            {
                yield return new ValidationResult("La personne doit avoir au moins 18 ans pour être embauchée");
            }

        }
    }*/

    public class Affectation
    {
        public int IdEmploye { get; set; }
        public string IdTerritoire { get; set; } = string.Empty;
    }

    public class Territoire
    {
        public string Id { get; set; } = string.Empty;
        public int IdRegion { get; set; }
        public string Nom { get; set; } = string.Empty;
        public virtual Region Region { get; set; } = null!;
    }

    public class Region
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;

        //En spécifiant cette propriété comme virtuelle, on pourra
        //charger les territoires de façon tardive, c’est-à-dire au moment de leur premier accès 
        //propriete de navigation, permet d'obtenir tous les territoires apartir de region
        public virtual List<Territoire> Territoires { get; set; }
    }
}
