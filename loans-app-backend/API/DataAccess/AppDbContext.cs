using API.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Thing> Things { get; set; }
        public AppDbContext(DbContextOptions options) : base(options) => this.Database.EnsureCreated();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany<Thing>()
                .WithOne(t => t.Category)
                .IsRequired();

            modelBuilder.Entity<Thing>()
                .HasMany<Loan>()
                .WithOne(l => l.Thing)
                .IsRequired();

            modelBuilder.Entity<Person>()
                .HasMany<Loan>()
                .WithOne(l => l.Person)
                .IsRequired();

            //modelBuilder.Entity<User>(u =>
            //{
            //    u.HasData
            //    (new User 
            //    { Id = 1, 
            //    UserName = "Admin",
            //    Password = "123"
            //    });
            //});

            modelBuilder.Entity<Category>(c =>
            {
                c.HasData(
                    new Category()
                    {
                        Id = 1,
                        Description = "libros",
                        CreationDate = new DateOnly(1997, 01, 22)
                    },
                    new Category()
                    {
                        Id = 2,
                        Description = "computación",
                        CreationDate = new DateOnly(1997, 01, 22)
                    },
                    new Category()
                    {
                        Id = 3,
                        Description = "audio",
                        CreationDate = new DateOnly(1997, 01, 22)
                    },
                    new Category()
                    {
                        Id = 4,
                        Description = "indumentaria",
                        CreationDate = new DateOnly(1997, 01, 22)
                    },
                    new Category()
                    {
                        Id = 5,
                        Description = "varios",
                        CreationDate = new DateOnly(1997, 01, 22)
                    });
            });
        }
    }
}
