using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        //The models that our application will use and generate tables for in the database
        public DbSet<Artifact> Artifacts { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<ArtifactMediaFile> ArtifactMediaFiles { get; set; }
        public DbSet<CatalogRecord> CatalogRecords { get; set; }
        public DbSet<CatalogNote> CatalogNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CatalogRecord>()
                .HasOne(cr => cr.SubmittedBy)
                .WithMany(u => u.SubmittedCatalogRecords)
                .HasForeignKey(cr => cr.SubmittedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CatalogRecord>()
                .HasOne(cr => cr.VerifiedBy)
                .WithMany(u => u.VerifiedCatalogRecords)
                .HasForeignKey(cr => cr.VerifiedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Artifact>()
                .Property(a => a.Type)
                .HasConversion<string>();
        }

    }
    
}
