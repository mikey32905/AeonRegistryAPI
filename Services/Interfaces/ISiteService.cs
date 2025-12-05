namespace AeonRegistryAPI.Services.Interfaces
{
    public interface ISiteService
    {
        Task<List<PublicSiteResponse>> GetAllSitesPublicAsync(CancellationToken ct);

        Task<PublicSiteResponse?> GetPublicSiteByIdAsync(int Id, CancellationToken ct);

        Task<List<PrivateSiteResponse>> GetAllSitesPrivateAsync(CancellationToken ct);

        Task<PrivateSiteResponse?> GetPrivateSiteByIdAsync(int Id, CancellationToken ct);
    }
}
