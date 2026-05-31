using Microsoft.EntityFrameworkCore;
using PersonService.Domain.Entities;

namespace PersonService.Infrastructure.Data
{
    public class PersonDbContext : DbContext
    {
        public PersonDbContext(DbContextOptions<PersonDbContext> options)
                : base(options) { }

        public DbSet<Person> Persons => Set<Person>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(e =>
            {
                e.HasKey(p => p.Id);

                e.OwnsOne(p => p.NationalCode, nav =>
                {
                    nav.Property(n => n.Value)
                    .HasColumnName(nameof(Person.NationalCode));
                    nav.HasIndex(n => n.Value).IsUnique();
                });

                e.OwnsOne(p => p.FirstName, nav =>
                {
                    nav.Property(n => n.Value)
                    .HasColumnName(nameof(Person.FirstName));
                });

                e.OwnsOne(p => p.LastName, nav =>
                {
                    nav.Property(n => n.Value)
                    .HasColumnName(nameof(Person.LastName));
                });

                e.OwnsOne(p => p.BirthDate, nav =>
                {
                    nav.Property(n => n.Value)
                    .HasColumnName(nameof(Person.BirthDate));
                });

                e.HasQueryFilter(p => !p.IsDeleted);

                e.HasIndex(p => p.IsDeleted);

            });
        }
    }
}

