using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Workflow.Domain.Entities;

namespace Workflow.Persistence;

public class WFContext(DbContextOptions<WFContext> options) : IdentityDbContext<Utilisateur, Role, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Service>()
            .HasMany(s => s.Employes)
            .WithOne(u => u.Service)
            .HasForeignKey(u => u.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Dossier>()
            .HasOne(d => d.EmployeTraitant)
            .WithMany()
            .HasForeignKey(d => d.EmployeTraitantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Dossier>()
            .HasOne(d => d.ServiceTraitant)
            .WithMany()
            .HasForeignKey(d => d.ServiceTraitantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ActionDossier>()
            .HasOne(a => a.Utilisateur)
            .WithMany()
            .HasForeignKey(a => a.UtilisateurId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ActionDossier>()
            .HasOne(a => a.Dossier)
            .WithMany(d => d.Historique)
            .HasForeignKey(a => a.DossierId);

        builder.Entity<Seance>()
            .HasMany(s => s.POJs)
            .WithOne(p => p.Seance)
            .HasForeignKey(p => p.SeanceId);

        builder.Entity<PointOrdreJour>()
            .HasOne(p => p.Dossier)
            .WithMany(d => d.POJs)
            .HasForeignKey(p => p.DossierId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Vote>()
            .HasOne(v => v.Utilisateur)
            .WithMany(u => u.Votes)
            .HasForeignKey(v => v.UtilisateurId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Vote>()
            .HasOne(v => v.PointOrdreJour)
            .WithMany(p => p.Votes)
            .HasForeignKey(v => v.PointOrdreJourId);

        builder.Entity<Notification>()
            .HasOne(n => n.Utilisateur)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UtilisateurId);
    }

    public DbSet<Service> Services { get; set; }
    public DbSet<Dossier> Dossiers { get; set; }
    public DbSet<ActionDossier> ActionsDossiers { get; set; }
    public DbSet<Seance> Seances { get; set; }
    public DbSet<PointOrdreJour> PointsOrdreJour { get; set; }
    public DbSet<Vote> Votes { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<Notification> Notifications { get; set; }
}
