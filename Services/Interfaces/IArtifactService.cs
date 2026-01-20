namespace AeonRegistryAPI.Services.Interfaces
{
    public interface IArtifactService
    {
        //public
        Task<List<PublicArtifactResponse>> GetPublicArtifactsAsync(CancellationToken ct);
        Task<List<PublicArtifactResponse>> GetPublicArtifactsBySiteAsync(int sideId, CancellationToken ct);

        //private
        Task<List<PrivateArtifactResponse>> GetPrivateArtifactsAsync(CancellationToken ct);
        Task<List<PrivateArtifactResponse>> GetPrivateArtifactsBySiteAsync(int siteId, CancellationToken ct);
        Task<PrivateArtifactResponse?> CreateArtifactAsync(CreateArtifactRequest request, CancellationToken ct);
    }
}
