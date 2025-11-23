using KeepFit.Backend.Domain;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;
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

        modelBuilder.Entity<ProgramExercise>()
            .HasOne(pe => pe.Exercise)             
            .WithMany(e => e.ProgramExercises)  
            .HasForeignKey(pe => pe.ExerciseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}