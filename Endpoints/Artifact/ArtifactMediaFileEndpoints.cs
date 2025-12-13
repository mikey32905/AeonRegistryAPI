using AeonRegistryAPI.Filters;
using AeonRegistryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace AeonRegistryAPI.Endpoints.Artifact
{
    public static class ArtifactMediaFileEndpoints
    {
       //groups
        public static IEndpointRouteBuilder MapArtifactMediaFileEndpoints(this IEndpointRouteBuilder route)
        {
            var publicGroup = route.MapGroup("/api/public/artifacts/images")
                 .WithTags("Artifact Media Files")
                 .AllowAnonymous()
                 .AddEndpointFilter<ExceptionHandlingFilter>();

            publicGroup.MapGet("/{id:int}", GetArtifactImage)
                .WithName(nameof(GetArtifactImage))
                .Produces<FileContentHttpResult>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get an Artifact Image")
                .WithDescription("Retrieves the binary image data for an artifact");

            var privateGroup = route.MapGroup("/api/private/artifacts/{artifactId}/images")
                .WithTags("Artifact Media - Private")
                .RequireAuthorization()
                .AddEndpointFilter<ExceptionHandlingFilter>();

            privateGroup.MapPost("", CreateArtifactMediaFile)
                .DisableAntiforgery()
                .WithName(nameof(CreateArtifactMediaFile))
                .Accepts<IFormFile>("multipart/form-data")
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Upload an Artifact Image - Private")
                .WithDescription("Uploads an image file and associates with an artifact.");

            return route;
        }
 



        //handlers
        private static async Task<Results<FileContentHttpResult, NotFound>>GetArtifactImage(int Id,
            ApplicationDbContext db,
            HttpResponse response,
            CancellationToken ct)
        {
            var image =  await db.ArtifactMediaFiles
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == Id);

            if (image is null || image.Data.Length == 0)    
            {
                return TypedResults.NotFound();
            }

            //optional caching header for a better performance
            response.Headers.CacheControl = "public,max-age=86400"; //cache for 1 day

            return TypedResults.File(image.Data, image.ContentType);
        }

        private static async Task<Results<Created, NotFound, BadRequest>> CreateArtifactMediaFile(int artifactId,
            IFormFile file,
            bool isPrimary,
            IArtifactMediaFileService service,
            CancellationToken ct)
        {
            if (file is null || file.Length == 0)
            {
                return TypedResults.BadRequest();
            }

            var media = await service.CreateArtifactMediaFileAsync(artifactId, file, isPrimary, ct);

            if (media is null)
            {
                return TypedResults.NotFound();
            }

            var location = $"/api/public/artifacts/images/{media.Id}";

            return TypedResults.Created(location);
        }

    }
}
