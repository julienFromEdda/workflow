using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Workflow.Application.Helpers;
using Workflow.Domain.Entities;
using Workflow.Persistence;

namespace Workflow.UI.Controllers;

[Authorize]
public class DocumentController(WFContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Gerer(string type, int objetId)
    {
        var documents = await context.Documents
            .Where(d => d.TypeObjet == type && d.ObjetId == objetId)
            .ToListAsync();

        ViewBag.TypeObjet = type;
        ViewBag.ObjetId = objetId;

        return PartialView("~/Views/Shared/DocumentsManager.cshtml", documents);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(string type, int objetId, IFormFile fichier)
    {
        if (fichier == null || fichier.Length == 0)
            return BadRequest();

        var url = DocumentHelper.SaveUploadedFile(fichier);

        var document = new Document
        {
            NomFichier = fichier.FileName,
            Url = url,
            TypeObjet = type,
            ObjetId = objetId
        };

        context.Documents.Add(document);
        await context.SaveChangesAsync();

        return await Gerer(type, objetId);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, string type, int objetId)
    {
        var doc = await context.Documents.FindAsync(id);
        if (doc == null || doc.TypeObjet != type || doc.ObjetId != objetId)
            return NotFound();

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", doc.Url.TrimStart('/'));
        if (System.IO.File.Exists(filePath))
            System.IO.File.Delete(filePath);

        context.Documents.Remove(doc);
        await context.SaveChangesAsync();

        return await Gerer(type, objetId);
    }
}
