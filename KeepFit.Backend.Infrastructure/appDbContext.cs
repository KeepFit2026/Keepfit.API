using KeepFit.Backend.Domain;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Domain.Models.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace KeepFit.Backend.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Exercise> Exercise { get; set; }
    public DbSet<FitnessProgram> FitnessProgram { get; set; }
    public DbSet<ProgramExercise> ProgramExercise { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProgramExercise>()
            .ToTable("ProgramExercises");
    
        modelBuilder.Entity<ProgramExercise>()
            .HasKey(pe => new { pe.ProgramId, pe.ExerciseId });
    
        modelBuilder.Entity<ProgramExercise>()
            .HasOne(pe => pe.Program)               
            .WithMany(p => p.ProgramExercises)     
            .HasForeignKey(pe => pe.ProgramId)
            .OnDelete(DeleteBehavior.Cascade);  
    }
}