using KeepFit.Backend.Domain.Models;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;
using KeepFit.Backend.Domain.Models.User;
using Microsoft.EntityFrameworkCore;

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
    public DbSet<Classroom> Classroom { get; set; }
    public DbSet<ClassroomUser> ClassroomUser { get; set; }
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
        
        
        modelBuilder.Entity<ClassroomUser>()
            .ToTable("ClassroomUsers");

        modelBuilder.Entity<ClassroomUser>()
            .HasKey(cu => new { cu.UserId, cu.ClassroomId });

        modelBuilder.Entity<ClassroomUser>()
            .HasOne(cu => cu.User)
            .WithMany(u => u.ClassroomUsers) 
            .HasForeignKey(cu => cu.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClassroomUser>()
            .HasOne(cu => cu.Classroom)
            .WithMany(c => c.ClassroomUsers) 
            .HasForeignKey(cu => cu.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}