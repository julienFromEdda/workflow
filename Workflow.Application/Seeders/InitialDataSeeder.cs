using Microsoft.AspNetCore.Identity;
using Workflow.Domain.Entities;
using Workflow.Domain.Enums;
using Workflow.Persistence;

namespace Workflow.Application.Seeders;

public class InitialDataSeeder(WFContext context, RoleManager<Role> roleManager, UserManager<Utilisateur> userManager)
{
    public async Task SeedDataAsync()
    {
        var services = new[] { "Administration Générale", "Ressources humaines", "Finances", "Environnement", "Culture & Tourisme", "Informatique" };
        foreach (var serviceName in services)
        {
            if (!context.Services.Any(x => x.Nom == serviceName))
            {
                context.Services.Add(new Service { Nom = serviceName });
                await context.SaveChangesAsync();
            }
        }

        var roles = new[] { "Administratif", "Greffe", "MembreCollege", "MembreConseil", "SuperAdmin" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new Role { Name = roleName });
            }
        }

        var adminEmail = "admin@workflow.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin == null)
        {
            var service = context.Services.First(x => x.Nom == "Administration Générale");

            admin = new Utilisateur
            {
                Email = adminEmail,
                UserName = adminEmail,
                Nom = "Workflow",
                Prenom = "Admin",
                EmailConfirmed = true,
                ServiceId = service.Id
            };

            var result = await userManager.CreateAsync(admin, "Admin123*");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }
        }

        //await SeedFakeDatas();
    }

    public async Task SeedFakeDatas()
    {
        // SEED UTILISATEURS SUPPLÉMENTAIRES
        var fakeUsers = new[]
        {
    ("jean.dupont@workflow.com", "Jean", "Dupont", "Administratif", "Informatique"),
    ("lucie.martin@workflow.com", "Lucie", "Martin", "Greffe", "Culture & Tourisme"),
    ("marc.leclerc@workflow.com", "Marc", "Leclerc", "MembreCollege", "Finances"),
    ("laura.dupuis@workflow.com", "Laura", "Dupuis", "MembreCollege", "Finances"),
    ("paul.moreau@workflow.com", "Paul", "Moreau", "MembreConseil", "Environnement"),
    ("emma.durand@workflow.com", "Emma", "Durand", "MembreConseil", "Ressources humaines")
};

        foreach (var (email, prenom, nom, role, serviceName) in fakeUsers)
        {
            if (await userManager.FindByEmailAsync(email) is null)
            {
                var service = context.Services.First(s => s.Nom == serviceName);
                var user = new Utilisateur
                {
                    Email = email,
                    UserName = email,
                    Prenom = prenom,
                    Nom = nom,
                    EmailConfirmed = true,
                    ServiceId = service.Id
                };

                var result = await userManager.CreateAsync(user, "Test123*");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
        await context.SaveChangesAsync();

        // SEED DOSSIERS
        var rnd = new Random();
        var serviceIds = context.Services.Select(s => s.Id).ToList();
        var users = context.Users.ToList();

            for (int i = 1; i <= 20; i++)
            {
                var serviceId = serviceIds[rnd.Next(serviceIds.Count)];
                var employe = users.FirstOrDefault(u => u.ServiceId == serviceId);
                var titre = $"Dossier {i}";
                var dossier = new Dossier
                {
                    Titre = titre,
                    Description = $"Description du {titre}",
                    Intervenant = rnd.Next(0, 2) == 1 ? $"Intervenant {i}" : null,
                    DateCreation = DateTime.Now.AddDays(-rnd.Next(30)),
                    DateModification = DateTime.Now,
                    Statut = StatutDossier.Cree,
                    ServiceTraitantId = serviceId,
                    EmployeTraitantId = employe?.Id
                };
                context.Dossiers.Add(dossier);
            }
            await context.SaveChangesAsync();
        // SEED SEANCES + POJs
            var seances = new List<Seance>();
            for (int i = 0; i < 2; i++)
            {
                seances.Add(new Seance
                {
                    Type = TypeSeance.College,
                    Date = DateTime.Today.AddDays(-10 + i),
                    Heure = new TimeSpan(9, 0, 0),
                    Statut = StatutSeance.Prevu
                });
                seances.Add(new Seance
                {
                    Type = TypeSeance.Conseil,
                    Date = DateTime.Today.AddDays(-5 + i),
                    Heure = new TimeSpan(14, 0, 0),
                    Statut = StatutSeance.Prevu
                });
            }
            context.Seances.AddRange(seances);
            await context.SaveChangesAsync();

            var dossiers = context.Dossiers.ToList();

            foreach (var seance in seances)
            {
                int nbPoj = rnd.Next(3, 6);
                for (int i = 1; i <= nbPoj; i++)
                {
                    var randomDossier = rnd.Next(0, 2) == 1 ? dossiers[rnd.Next(dossiers.Count)] : null;
                    var poj = new PointOrdreJour
                    {
                        Titre = $"POJ {i} ({seance.Type})",
                        Description = $"Point de l'ordre du jour numéro {i}",
                        Decision = rnd.Next(0, 2) == 1 ? $"Décision automatique {i}" : null,
                        Statut = (StatutPOJ)rnd.Next(0, 4),
                        SeanceId = seance.Id,
                        DossierId = randomDossier?.Id
                    };
                    context.PointsOrdreJour.Add(poj);
                }
            }
            await context.SaveChangesAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        // Supprimer les notifications
        context.Notifications.RemoveRange(context.Notifications);

        // Supprimer les votes
        context.Votes.RemoveRange(context.Votes);

        // Supprimer les documents
        context.Documents.RemoveRange(context.Documents);

        // Supprimer les actions liées aux dossiers
        context.ActionsDossiers.RemoveRange(context.ActionsDossiers);

        // Supprimer les POJs
        context.PointsOrdreJour.RemoveRange(context.PointsOrdreJour);

        // Supprimer les séances
        context.Seances.RemoveRange(context.Seances);

        // Supprimer les dossiers
        context.Dossiers.RemoveRange(context.Dossiers);

        // Supprimer les utilisateurs
        context.Users.RemoveRange(context.Users);

        // Supprimer les services
        context.Services.RemoveRange(context.Services);

        await context.SaveChangesAsync();
    }

}
