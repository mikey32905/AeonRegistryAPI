using AeonRegistryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Services
{
    public class SiteService(ApplicationDbContext db) : ISiteService
    {

        public async Task<List<PublicSiteResponse>> GetAllSitesPublicAsync(CancellationToken ct)
        {
            return await db.Sites
                .AsNoTracking()
                .Select(s => new PublicSiteResponse()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Location = s.Location,
                    Coordinates = s.Coordinates,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Description = s.Description,
                    PublicNarrative = s.PublicNarrative
                })
                .ToListAsync(ct);
        }

        public Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int Id, CancellationToken ct)
        {
            return db.Sites
                .AsNoTracking()
                .Where(s => s.Id == Id)
                .Select(s => new PublicSiteResponse()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Location = s.Location,
                    Coordinates = s.Coordinates,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Description = s.Description,
                    PublicNarrative = s.PublicNarrative
                })
                .FirstOrDefaultAsync(ct);
        }

        public async Task<List<PrivateSiteResponse>> GetAllSitesPrivateAsync(CancellationToken ct)
        {
            return await db.Sites
                .AsNoTracking()
                .Select(s => new PrivateSiteResponse()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Location = s.Location,
                    Coordinates = s.Coordinates,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Description = s.Description,
                    PublicNarrative = s.PublicNarrative,
                    AeonNarrative = s.AeonNarrative
                })
                .ToListAsync(ct);
        }

        public Task<PrivateSiteResponse?> GetPrivateSiteByIdAsync(int Id, CancellationToken ct)
        {
            return db.Sites
                .AsNoTracking()
                .Where(s => s.Id == Id)
                .Select(s => new PrivateSiteResponse()
                {
                    Id = s.Id,
                    Name = s.Name,
                    Location = s.Location,
                    Coordinates = s.Coordinates,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Description = s.Description,
                    PublicNarrative = s.PublicNarrative,
                    AeonNarrative = s.AeonNarrative
                })
                .FirstOrDefaultAsync(ct);
        }
    }
}
