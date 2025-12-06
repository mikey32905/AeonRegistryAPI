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

        public async Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int Id, CancellationToken ct)
        {
            return await db.Sites
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

        public async Task<PrivateSiteResponse?> GetPrivateSiteByIdAsync(int Id, CancellationToken ct)
        {
            return await db.Sites
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

        public async Task<PrivateSiteResponse> CreateSiteAsync(CreateSiteRequest request, CancellationToken ct)
        {
            var site = new Site
            {
                Name = request.Name,
                Location = request.Location,
                Coordinates = request.Coordinates,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Description = request.Description,
                PublicNarrative = request.PublicNarrative,
                AeonNarrative = request.AeonNarrative
            };

            db.Sites.Add(site);
            await db.SaveChangesAsync(ct);

            return new PrivateSiteResponse
            {
                Id = site.Id,
                Name = site.Name,
                Location = site.Location,
                Coordinates = site.Coordinates,
                Latitude = site.Latitude,
                Longitude = site.Longitude,
                Description = site.Description,
                PublicNarrative = site.PublicNarrative,
                AeonNarrative = site.AeonNarrative
            };
        }

        public async Task<bool> UpdateSiteAsync(int Id, UpdateSiteRequest request, CancellationToken ct)
        {
           var site = await db.Sites.FindAsync(Id , ct);

            if (site == null)
            {
                return false;
            }

            site.Name = request.Name;
            site.Location = request.Location;
            site.Coordinates = request.Coordinates;
            site.Latitude = request.Latitude;
            site.Longitude = request.Longitude;
            site.Description = request.Description;
            site.PublicNarrative = request.PublicNarrative;
            site.AeonNarrative = request.AeonNarrative;

            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteSiteAsync(int Id, CancellationToken ct)
        {
            var site = await db.Sites.FindAsync(Id, ct);

            if (site == null)
            {
                return false;
            }

            db.Sites.Remove(site);
            await db.SaveChangesAsync(ct);

            return true;
        }
    }
}
