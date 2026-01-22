namespace AeonRegistryAPI.Models.Request
{
    public class UpdateArtifactRequest
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string CatalogNumber { get; set; } = string.Empty;
        [Required, MaxLength(2000)]
        public string Description { get; set; } = string.Empty;
        [Required, MaxLength(2000)]
        public string PublicNarrative { get; set; } = string.Empty;
        [Required]
        public string Type { get; set; } = string.Empty; //artifact type enum as string

        [Required]
        public int SiteId { get; set; }

        [Required]
        public DateTime DateDiscovered { get; set; } = DateTime.UtcNow;
    }
}
