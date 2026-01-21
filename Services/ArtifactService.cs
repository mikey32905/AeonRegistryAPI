using AeonRegistryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Services
{
    public class ArtifactService(ApplicationDbContext db) : IArtifactService
    {
        public async Task<PrivateArtifactResponse?> CreateArtifactAsync(CreateArtifactRequest request, CancellationToken ct)
        {
            var site = await db.Sites.FindAsync(request.SiteId, ct);

            if (site == null)
            {
                return null;
            }

            if (!Enum.TryParse<ArtifactType>(request.Type, out var artifactType))
            {
               throw new ArgumentException("Invalid artifact type");
            }

            var artifact = new Artifact
            {
                Name = request.Name,
                CatalogNumber = request.CatalogNumber,
                Description = request.Description,
                PublicNarrative = request.PublicNarrative,
                DateDiscovered = request.DateDiscovered,
                Type = request.Type.ToString(),
                SiteId = request.SiteId
            };

            db.Artifacts.Add(artifact);
            await db.SaveChangesAsync(ct);

            return new PrivateArtifactResponse
            {
                Id = artifact.Id,
                Name = artifact.Name,
                CatalogNumber = artifact.CatalogNumber,
                Description = artifact.Description,
                PublicNarrative = artifact.PublicNarrative,
                DateDiscovered = artifact.DateDiscovered,
                Type = artifact.Type.ToString(),
                SiteId = artifact.SiteId,
                SiteName = site.Name,
                PrimaryImageUrl = ""
            };
        }

        //private
        public async Task<List<PrivateArtifactResponse>> GetPrivateArtifactsAsync(CancellationToken ct)
        {
            return await db.Artifacts
                .AsNoTracking()
                .Include(a => a.Site)
                .Include(a => a.MediaFiles)
                .Select(a => new PrivateArtifactResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    CatalogNumber = a.CatalogNumber,
                    Description = a.Description,
                    PublicNarrative = a.PublicNarrative,
                    DateDiscovered = a.DateDiscovered,
                    Type = a.Type.ToString(),
                    SiteId = a.SiteId,
                    SiteName = a.Site != null ? a.Site.Name : String.Empty,
                    PrimaryImageUrl = a.MediaFiles
                    .Where(m => m.IsPrimary)
                    .Select(m => $"/api/public/artifacts/images/{m.Id}")
                    .FirstOrDefault()
                }).ToListAsync(ct);
        }

        public async Task<List<PrivateArtifactResponse>> GetPrivateArtifactsBySiteAsync(int siteId, CancellationToken ct)
        {
            return await db.Artifacts
                .AsNoTracking()
                .Include(a => a.Site)
                .Include(a => a.MediaFiles)
                .Where(a => a.SiteId == siteId)
                .Select(a => new PrivateArtifactResponse
                {
                    Id = a.Id,
                    Name = a.Name,
                    CatalogNumber = a.CatalogNumber,
                    Description = a.Description,
                    PublicNarrative = a.PublicNarrative,
                    DateDiscovered = a.DateDiscovered,
                    Type = a.Type.ToString(),
                    SiteId = a.SiteId,
                    SiteName = a.Site != null ? a.Site.Name : String.Empty,
                    PrimaryImageUrl = a.MediaFiles
                    .Where(m => m.IsPrimary)
                    .Select(m => $"/api/public/artifacts/images/{m.Id}")
                    .FirstOrDefault()
                }).ToListAsync(ct);
        }

        public async Task<PrivateArtifactResponse?> GetPrivateArtifactByIdAsync(int id, CancellationToken ct)
        {
            var artifact = await db.Artifacts
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new PrivateArtifactResponse
                {
                    Id = a.Id,
                    Name = a.Name!,
                    CatalogNumber = a.CatalogNumber!,
                    PublicNarrative = a.PublicNarrative,
                    Description = a.Description,
                    DateDiscovered = a.DateDiscovered,
                    Type = a.Type.ToString(),
                    SiteName = a.Site!.Name!,
                    PrimaryImageUrl = a.MediaFiles
                    .Where(m => m.IsPrimary)
                    .Select(m => $"/api/public/artifacts/images/{m.Id}")
                    .FirstOrDefault()
                })
                .FirstOrDefaultAsync(ct);

            return artifact;
        }

        //public

        public async Task<List<PublicArtifactResponse>> GetPublicArtifactsAsync(CancellationToken ct)
        {
            return await db.Artifacts
                .AsNoTracking()
                .Include(a => a.Site)
                .Include(a => a.MediaFiles)
                .Select(a => new PublicArtifactResponse
                {
                   Id = a.Id,
                   Name = a.Name,
                   CatalogNumber = a.CatalogNumber,
                   PublicNarrative = a.PublicNarrative,
                   DateDiscovered = a.DateDiscovered,
                   Type = a.Type.ToString(),
                   SiteId = a.SiteId,
                   SiteName = a.Site != null ? a.Site.Name : String.Empty,
                   PrimaryImageUrl = a.MediaFiles
                    .Where(m => m.IsPrimary)
                    .Select(m => $"/api/public/artifacts/images/{m.Id}")
                    .FirstOrDefault()
                }).ToListAsync(ct);
        }

        public async Task<List<PublicArtifactResponse>> GetPublicArtifactsBySiteAsync(int siteId, CancellationToken ct)
        {
            var siteExists = await db.Sites
                .AsNoTracking()
                .AnyAsync(s => s.Id == siteId, ct);

            if  (!siteExists)
            {
                return new List<PublicArtifactResponse>();
            }

            return await db.Artifacts
                .AsNoTracking()
                .Include(a => a.Site)
                .Include(a => a.MediaFiles)
                .Where(a => a.SiteId == siteId)
                .Select(a => new PublicArtifactResponse
                 {
                     Id = a.Id,
                     Name = a.Name,
                     CatalogNumber = a.CatalogNumber,
                     PublicNarrative = a.PublicNarrative,
                     DateDiscovered = a.DateDiscovered,
                     Type = a.Type.ToString(),
                     SiteId = a.SiteId,
                     SiteName = a.Site != null ? a.Site.Name : String.Empty,
                     PrimaryImageUrl = a.MediaFiles
                    .Where(m => m.IsPrimary)
                    .Select(m => $"/api/public/artifacts/images/{m.Id}")
                    .FirstOrDefault()
                 }).ToListAsync(ct);

        }

        public async Task<PublicArtifactResponse?> GetPublicArtifactByIdAsync(int id, CancellationToken ct)
        {
            var artifact = await db.Artifacts
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new PublicArtifactResponse
                {
                    Id = a.Id,
                    Name = a.Name!,
                    CatalogNumber = a.CatalogNumber!,
                    PublicNarrative = a.PublicNarrative,
                    DateDiscovered = a.DateDiscovered,
                    Type = a.Type.ToString(),
                    SiteName = a.Site!.Name!,
                    PrimaryImageUrl = a.MediaFiles
                    .Where(m => m.IsPrimary)
                    .Select(m => $"/api/public/artifacts/images/{m.Id}")
                    .FirstOrDefault()
                })
                .FirstOrDefaultAsync(ct);

            return artifact;
        }


    }
}
