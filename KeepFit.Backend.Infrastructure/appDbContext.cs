using KeepFit.Backend.Domain.Models;
using KeepFit.Backend.Domain.Models.Chats;
using KeepFit.Backend.Domain.Models.Training;
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
    
    public DbSet<Seance> Seance { get; set; }
    public DbSet<SeanceExercise> SeanceExercise { get; set; }
    public DbSet<MuscleGroup> MuscleGroup { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Role> Role { get; set; }
    
    public DbSet<Classroom> Classroom { get; set; }
    public DbSet<ClassroomUser> ClassroomUser { get; set; }
    
    public DbSet<Conversation> Conversation { get; set; }
    public DbSet<Message> Message { get; set; }
    public DbSet<ConversationParticipant> ConversationParticipant { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
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
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(u => u.Role)         
                .WithMany(r => r.Users)      
                .HasForeignKey(u => u.RoleId) 
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.MessagesSent)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}