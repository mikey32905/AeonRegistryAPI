using AeonRegistryAPI.Filters;
using AeonRegistryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AeonRegistryAPI.Endpoints.Artifact
{
    public static class ArtifactEndpoints
    {
        //groups

        public static IEndpointRouteBuilder MapArtifactEndpoints(this IEndpointRouteBuilder route)
        {
            var publicGroup = route.MapGroup("api/public/artifacts")
                .WithTags("Artifacts - Public")
                 .AddEndpointFilter<ExceptionHandlingFilter>()
                 .AllowAnonymous();

            publicGroup.MapGet("", GetPublicArtifacts)
                .WithName(nameof(GetPublicArtifacts))
                .WithSummary("Get all public artifacts")
                .WithDescription("Returns all artifacts with public fields and images")
                .Produces<List<PublicArtifactResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);



            var privateGroup = route.MapGroup("api/private/artifacts")
                .WithTags("Artifacts - Private")
                 .AddEndpointFilter<ExceptionHandlingFilter>()
                 .RequireAuthorization();


            privateGroup.MapGet("", GetPrivateArtifacts)
                .WithName(nameof(GetPrivateArtifacts))
                .WithSummary("Get all private artifacts")
                .WithDescription("Returns all artifacts with private fields and images")
                .Produces<List<PrivateArtifactResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);



            privateGroup.MapPost("", CreateArtifact)
                .WithName(nameof(CreateArtifact))
                .WithSummary("Create a new artifact record")
                .WithDescription("Creates a new artifact record with private data")
                .Produces<PrivateArtifactResponse>(StatusCodes.Status201Created)
                .ProducesValidationProblem()
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            return route;
        }


        //handlers
        private static async Task<Results<Ok<List<PublicArtifactResponse>>, NotFound>> GetPublicArtifacts(
            IArtifactService service,
            CancellationToken ct)
        {
            var artifacts = await service.GetPublicArtifactsAsync(ct);

            if (artifacts is null || artifacts.Count < 1)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(artifacts);
        }


        private static async Task<Results<Ok<List<PrivateArtifactResponse>>, NotFound>> GetPrivateArtifacts(
            IArtifactService service,
            CancellationToken ct)
        {
            var artifacts = await service.GetPrivateArtifactsAsync(ct);

            if (artifacts is null || artifacts.Count < 1)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(artifacts);
        }

 
        private static async Task<Results<Created<PrivateArtifactResponse>, NotFound>>  CreateArtifact(
            CreateArtifactRequest request,
            IArtifactService service,
            CancellationToken ct)
        {
            var created = await service.CreateArtifactAsync(request, ct);
            if (created is null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Created($"/api/private/artifacts/{created.Id}", created);
        }

    }
}
