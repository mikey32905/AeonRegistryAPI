using AeonRegistryAPI.Models.Request;

namespace AeonRegistryAPI.Services.Interfaces
{
    public interface ISiteService
    {
        Task<List<PublicSiteResponse>> GetAllSitesPublicAsync(CancellationToken ct);

        Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int Id, CancellationToken ct);

        Task<List<PrivateSiteResponse>> GetAllSitesPrivateAsync(CancellationToken ct);

        Task<PrivateSiteResponse?> GetPrivateSiteByIdAsync(int Id, CancellationToken ct);

        Task<PrivateSiteResponse> CreateSiteAsync(CreateSiteRequest request, CancellationToken ct);

        Task<bool> UpdateSiteAsync(int Id, UpdateSiteRequest request, CancellationToken ct);

        Task<bool> DeleteSiteAsync(int Id, CancellationToken ct);
    }
}
