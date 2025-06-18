namespace Workflow.Application.Helpers;

public static class DocumentHelper
{
    public static string SaveUploadedFile(IFormFile fichier)
    {
        var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsPath);

        var originalName = Path.GetFileNameWithoutExtension(fichier.FileName);
        var extension = Path.GetExtension(fichier.FileName);
        var uniqueName = $"{originalName}_{Guid.NewGuid():N}{extension}";
        var filePath = Path.Combine(uploadsPath, uniqueName);

        using var stream = new FileStream(filePath, FileMode.Create);
        fichier.CopyTo(stream);

        return $"/uploads/{uniqueName}";
    }
}
