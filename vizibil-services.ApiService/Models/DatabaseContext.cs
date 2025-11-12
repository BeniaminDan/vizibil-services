
using vizibil_api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace vizibil_api.Models;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public bool AutomaticMigrationsEnabled { get; set; } = true;
    public DbSet<Contact> Contacts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>()
            .Property(b => b.Id)
            .HasDefaultValueSql("uuid_generate_v4()");
        
        modelBuilder.Entity<Contact>()
            .Property(b => b.Date)
            .HasDefaultValueSql("now()");
    }
    
    public void InitializeDatabase()
    {
        Console.WriteLine("Initializing database...");
        Database.ExecuteSqlRaw("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");
    }
}