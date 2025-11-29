namespace AeonRegistryAPI.Models
{
    public class ArtifactMediaFile
    {
        public int Id { get; set; }
        public int ArtifactId { get; set; }
        public Artifact? Artifact { get; set; } = null!;
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = "image/jpeg"; // e.g., "image", "video"
        public byte[] Data { get; set; }  = Array.Empty<byte>(); // Binary data of the media file

        //optional
        public bool IsPrimary { get; set; } = false; // Indicates if this is the primary media file for the artifact
    }
}
