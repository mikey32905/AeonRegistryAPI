namespace AeonRegistryAPI.Models
{
    public class CatalogNote
    {
        public int Id { get; set; }

        [Required]
        public int CatalogRecordId { get; set; }
        public CatalogRecord? CatalogRecord { get; set; } = null!;


        public string AuthorId { get; set; } = string.Empty; //foreign key to ApplicationUser
        public ApplicationUser? Author { get; set; } = null!;


        [Required, MaxLength(2000) ] 
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
