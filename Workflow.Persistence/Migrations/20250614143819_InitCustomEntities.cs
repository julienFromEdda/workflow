using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Workflow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitCustomEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "AspNetUsers",
                newName: "Prenom");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "AspNetUsers",
                newName: "ServiceId");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "AspNetUsers",
                newName: "Nom");

            migrationBuilder.AddColumn<string>(
                name: "Nom",
                table: "Services",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dossiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titre = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DateCreation = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateModification = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false),
                    Intervenant = table.Column<string>(type: "TEXT", nullable: true),
                    EmployeTraitantId = table.Column<string>(type: "TEXT", nullable: true),
                    ServiceTraitantId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dossiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dossiers_AspNetUsers_EmployeTraitantId",
                        column: x => x.EmployeTraitantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Dossiers_Services_ServiceTraitantId",
                        column: x => x.ServiceTraitantId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UtilisateurId = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: true),
                    LienObjet = table.Column<string>(type: "TEXT", nullable: true),
                    Lu = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Seances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Heure = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ActionsDossiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Action = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    UtilisateurId = table.Column<string>(type: "TEXT", nullable: true),
                    DossierId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionsDossiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionsDossiers_AspNetUsers_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionsDossiers_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PointsOrdreJour",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titre = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Decision = table.Column<string>(type: "TEXT", nullable: true),
                    Statut = table.Column<int>(type: "INTEGER", nullable: false),
                    DossierId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeanceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointsOrdreJour", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PointsOrdreJour_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PointsOrdreJour_Seances_SeanceId",
                        column: x => x.SeanceId,
                        principalTable: "Seances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NomFichier = table.Column<string>(type: "TEXT", nullable: true),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    ObjetId = table.Column<int>(type: "INTEGER", nullable: false),
                    TypeObjet = table.Column<string>(type: "TEXT", nullable: true),
                    DossierId = table.Column<int>(type: "INTEGER", nullable: true),
                    PointOrdreJourId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Dossiers_DossierId",
                        column: x => x.DossierId,
                        principalTable: "Dossiers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_PointsOrdreJour_PointOrdreJourId",
                        column: x => x.PointOrdreJourId,
                        principalTable: "PointsOrdreJour",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UtilisateurId = table.Column<string>(type: "TEXT", nullable: true),
                    PointOrdreJourId = table.Column<int>(type: "INTEGER", nullable: false),
                    Valeur = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_AspNetUsers_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votes_PointsOrdreJour_PointOrdreJourId",
                        column: x => x.PointOrdreJourId,
                        principalTable: "PointsOrdreJour",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ServiceId",
                table: "AspNetUsers",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsDossiers_DossierId",
                table: "ActionsDossiers",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionsDossiers_UtilisateurId",
                table: "ActionsDossiers",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_DossierId",
                table: "Documents",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PointOrdreJourId",
                table: "Documents",
                column: "PointOrdreJourId");

            migrationBuilder.CreateIndex(
                name: "IX_Dossiers_EmployeTraitantId",
                table: "Dossiers",
                column: "EmployeTraitantId");

            migrationBuilder.CreateIndex(
                name: "IX_Dossiers_ServiceTraitantId",
                table: "Dossiers",
                column: "ServiceTraitantId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UtilisateurId",
                table: "Notifications",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_PointsOrdreJour_DossierId",
                table: "PointsOrdreJour",
                column: "DossierId");

            migrationBuilder.CreateIndex(
                name: "IX_PointsOrdreJour_SeanceId",
                table: "PointsOrdreJour",
                column: "SeanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_PointOrdreJourId",
                table: "Votes",
                column: "PointOrdreJourId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UtilisateurId",
                table: "Votes",
                column: "UtilisateurId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Services_ServiceId",
                table: "AspNetUsers",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Services_ServiceId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ActionsDossiers");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "PointsOrdreJour");

            migrationBuilder.DropTable(
                name: "Dossiers");

            migrationBuilder.DropTable(
                name: "Seances");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ServiceId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Nom",
                table: "Services");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "AspNetUsers",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "Prenom",
                table: "AspNetUsers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "AspNetUsers",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Services",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: true);
        }
    }
}
