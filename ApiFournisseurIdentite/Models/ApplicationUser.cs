using Microsoft.AspNetCore.Identity;

namespace ApiFournisseurIdentite.Models
{
    public class ApplicationUser : IdentityUser<int>
    {

        public string Fonction { get; set; } = "";
    }
    
   
}
