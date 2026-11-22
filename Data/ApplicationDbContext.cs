using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using proiect_dotnet.Models;

namespace proiect_dotnet.Data
{
    // The database context. IdentityDbContext<ApplicationUser> automatically creates
    // the Identity tables (AspNetUsers, AspNetRoles, etc.) using our own user class.
    // TODO (Pas 3): Ce tabele creeaza IdentityDbContext singur, fara sa le scrii tu?
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for StudyGroup, Post, etc. will be added here.

        // TODO (Pas 3): De ce este obligatoriu base.OnModelCreating? Aici vei adauga relatiile Restrict pentru Follow/Reaction.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // required for Identity

            // Relationship configuration (Restrict) is added in the Follow/Reaction task.
        }
    }
}
