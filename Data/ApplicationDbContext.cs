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
        public DbSet<StudyGroup> StudyGroups {get; set;}
        public DbSet<GroupMembership> GroupMemberships {get; set;}
        public DbSet<GroupMessage> GroupMessages {get; set;}
        public DbSet<Post> Posts {get; set;}
        public DbSet<Comment> Comments {get; set;}
        // TODO (Pas 3): De ce este obligatoriu base.OnModelCreating? Aici vei adauga relatiile Restrict pentru Follow/Reaction.
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // required for Identity
            
            // Relationship configuration (Restrict) is added in the Follow/Reaction task.
            builder.Entity<GroupMembership>()
                .HasIndex(m => new {m.GroupId, m.UserId})
                .IsUnique();

            //fks to AspNetUsers on restrict, otherwise multiple cascade paths which is a no no

            builder.Entity<StudyGroup>()
                .HasOne(g => g.Moderator).WithMany()
                .HasForeignKey(g => g.ModeratorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GroupMembership>()
                .HasOne(m => m.User).WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<GroupMessage>()
                .HasOne(m => m.User).WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Post>()
                .HasOne(p => p.User).WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Comment>()
                .HasOne(c => c.User).WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // deleting a post deletes its comments
            builder.Entity<Comment>()
                .HasOne(c => c.Post).WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
