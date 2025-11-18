using KeepFit.Backend.Domain;
using KeepFit.Backend.Domain.Models.Exercise;
using KeepFit.Backend.Domain.Models.Program;
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
}