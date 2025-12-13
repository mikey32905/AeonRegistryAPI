using AeonRegistryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Services
{
    public class ArtifactMediaFileService(ApplicationDbContext db) : IArtifactMediaFileService
    {
        public async Task<ArtifactMediaFile?> CreateArtifactMediaFileAsync(int artifactId, IFormFile file, bool isPrimary, CancellationToken ct)
        {
           var artifact = await db.Artifacts.FindAsync(artifactId, ct);

            if (artifact is null)
            {
                return null;
            }

            //validate the input
            if (file is null || file.Length == 0)
            {
                throw new ArgumentException("File is null or empty");
            }

            if (isPrimary)
            {
                var existingPrimary = await db.ArtifactMediaFiles
                   .Where(m => m.ArtifactId == artifactId && m.IsPrimary == true)
                   .ToListAsync(ct);

                foreach (var mediaFile in existingPrimary) 
                { 
                    mediaFile.IsPrimary = false;
                }

            }

            //convert IFormFile to byte array
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms, ct);

            var data = ms.ToArray();
            var newMediaFile = new ArtifactMediaFile
            {
                ArtifactId = artifactId,
                FileName = file.FileName,
                ContentType = file.ContentType,
                Data = data,
                IsPrimary = isPrimary
            };

            db.ArtifactMediaFiles.Add(newMediaFile);
            await db.SaveChangesAsync(ct);

            return newMediaFile;

        }
    }
}
