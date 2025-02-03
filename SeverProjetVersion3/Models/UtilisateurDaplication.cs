using Microsoft.AspNetCore.Identity;

namespace SeverProjetVersion3.Models
{
    public class UtilisateurDaplication : IdentityUser<int>
    {
        public string Fonction { get; set; } = "";
    }
}
