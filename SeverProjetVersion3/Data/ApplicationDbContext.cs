using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeverProjetVersion3.Models;

namespace SeverProjetVersion3.Data
{
    public class ApplicationDbContext : IdentityDbContext<UtilisateurDaplication, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UtilisateurDaplication>(entity =>
            {
                entity.Property(e => e.Fonction).HasMaxLength(40)
                   .IsUnicode(false).HasDefaultValue("");
            });
        }
    }
}
