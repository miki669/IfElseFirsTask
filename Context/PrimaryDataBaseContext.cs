using IfElseFirsTask.Context.Model;
using Microsoft.EntityFrameworkCore;


namespace IfElseFirsTask.Context;

public class PrimaryDataBaseContext : DbContext
{
    
    public DbSet<Account?> Account { get; set; }
    public DbSet<Animal> Animal { get; set; }
    public DbSet<AnimalType> AnimalType { get; set; }
    public DbSet<AnimalVisitedLocation> AnimalVisitedLocation { get; set; }
    public DbSet<LocationPoint> LocationPoint { get; set; }
    public DbSet<AccountToken> Tokens { get; set; }
    
    
    public PrimaryDataBaseContext(DbContextOptions<PrimaryDataBaseContext> options) : base(options)
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
       
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}