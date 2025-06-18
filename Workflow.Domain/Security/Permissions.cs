namespace Workflow.Domain.Security;

public static class Permissions
{
    public static class Seance
    {
        public const string Lire = "Permissions.Seance.Lire";
        public const string Modifier = "Permissions.Seance.Modifier";
    }

    public static class POJ
    {
        public const string Creer = "Permissions.POJ.Creer";
        public const string Modifier = "Permissions.POJ.Modifier";
        public const string Supprimer = "Permissions.POJ.Supprimer";
    }

    public static class Dossier
    {
        public const string Acces = "Permissions.Dossier.Acces";
        public const string GererDocuments = "Permissions.Dossier.GererDocuments";
    }

    public static class Utilisateur
    {
        public const string Gerer = "Permissions.Utilisateur.Gerer";
    }

    public static class Vote
    {
        public const string Voter = "Permissions.Vote.Voter";
    }

    public static Dictionary<string, List<string>> GetRolePermissions()
    {
        return new Dictionary<string, List<string>>
        {
            ["SuperAdmin"] =
            [
                Seance.Lire, Seance.Modifier,
                POJ.Creer, POJ.Modifier, POJ.Supprimer,
                Dossier.Acces, Dossier.GererDocuments,
                Vote.Voter,
                Utilisateur.Gerer
            ],
            ["Administratif"] =
            [
                Seance.Lire,
                Dossier.Acces,
                Dossier.GererDocuments
            ],
            ["Greffe"] =
            [
                Seance.Lire, Seance.Modifier,
                POJ.Creer, POJ.Modifier, POJ.Supprimer,
                Dossier.Acces, Dossier.GererDocuments
            ],
            ["MembreCollege"] =
            [
                Seance.Lire
            ],
            ["MembreConseil"] =
            [
                Seance.Lire,
                Vote.Voter
            ]
        };
    }
}
