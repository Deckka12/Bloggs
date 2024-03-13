
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DBContex.Models
{
    public class Context : IdentityDbContext<IdentityUser>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"workstation id=Bloggs1.mssql.somee.com;packet size=4096;user id=Daniil111_SQLLogin_1;pwd=aaeeayvcxq;data source=Bloggs1.mssql.somee.com;persist security info=False;initial catalog=Bloggs1;TrustServerCertificate=True");
        }
        public Context (DbContextOptions<Context> options)
            : base(options) {
            //Database.EnsureDeleted();
           Database.EnsureCreated();
        }
        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);
            foreach(var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            modelBuilder.Entity<Article>()
                .HasOne<IdentityUser>(a => a.Author)
                .WithMany()
                .HasForeignKey(a => a.AuthorId);
        }
    }
}
