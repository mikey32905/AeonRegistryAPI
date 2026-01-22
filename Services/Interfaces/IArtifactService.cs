namespace AeonRegistryAPI.Services.Interfaces
{
    public interface IArtifactService
    {
        //public
        Task<List<PublicArtifactResponse>> GetPublicArtifactsAsync(CancellationToken ct);
        Task<List<PublicArtifactResponse>> GetPublicArtifactsBySiteAsync(int sideId, CancellationToken ct);
        Task<PublicArtifactResponse?> GetPublicArtifactByIdAsync(int id, CancellationToken ct);

        //private
        Task<List<PrivateArtifactResponse>> GetPrivateArtifactsAsync(CancellationToken ct);
        Task<List<PrivateArtifactResponse>> GetPrivateArtifactsBySiteAsync(int siteId, CancellationToken ct);
        Task<PrivateArtifactResponse?> CreateArtifactAsync(CreateArtifactRequest request, CancellationToken ct);
        Task<PrivateArtifactResponse?> GetPrivateArtifactByIdAsync(int id, CancellationToken ct);
        Task<bool> UpdateArtifactAsync( int artifactId, UpdateArtifactRequest request, CancellationToken ct);
        Task<bool> DeleteArtifactAsync( int artifactId, CancellationToken ct);
    }
}
