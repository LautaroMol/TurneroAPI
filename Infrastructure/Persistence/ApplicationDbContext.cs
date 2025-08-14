using Microsoft.EntityFrameworkCore;
using TurneroAPI.Application.Interfaces;
using TurneroAPI.Domain.Entities;
using System.Reflection;

namespace TurneroAPI.Infrastructure.Persistence;

/// <summary>
/// EF Core DbContext for the application.
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Relación: Un usuario puede ser paciente en muchos turnos
        builder.Entity<User>()
            .HasMany(u => u.TurnosComoPaciente)
            .WithOne(t => t.Patient)
            .HasForeignKey(t => t.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relación: Un usuario puede ser médico en muchos turnos
        builder.Entity<User>()
            .HasMany(u => u.TurnosComoMedico)
            .WithOne(t => t.Doctor)
            .HasForeignKey(t => t.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
