using AeonRegistryAPI.Filters;
using AeonRegistryAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AeonRegistryAPI.Endpoints.Sites
{
    public static class SiteEndPoints
    {
        //Endpoint Groups for Sites

        public static IEndpointRouteBuilder MapSiteEndpoints(this IEndpointRouteBuilder route) 
        {
            //first id the group
            var publicGroup = route.MapGroup("/api/public/sites")
                .AllowAnonymous()
                .WithSummary("Public Site Endpoints")
                .WithDescription("Endpoints that expose public site data")
                .WithTags("Sites - Public")
                .AddEndpointFilter<ExceptionHandlingFilter>();

            //then endpoints
            publicGroup.MapGet("", GetAllPublicSites)
                .WithName(nameof(GetAllPublicSites))
                .Produces<List<PublicSiteResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get All Sites (Public)")
                .WithDescription("Returns all sites with their public data only.");

            publicGroup.MapGet("/{id:int}", GetPublicSiteById)
                .WithName(nameof(GetPublicSiteById))
                .Produces<PublicSiteResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get Site By Id (Public)")
                .WithDescription("Returns a single site with public data.");

            var privateGroup = route.MapGroup("/api/private/sites")
                .RequireAuthorization()
                .WithTags("Sites - Private")
                .WithSummary("Authorized Site Endpoints")
                .WithDescription("Endpoints that require authentication.");

            privateGroup.MapGet("", GetAllPrivateSites)
                .WithName(nameof(GetAllPrivateSites))
                .Produces<List<PrivateSiteResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get All Sites - Private")
                .WithDescription("Returns all sites with info that requires authorization.");

            privateGroup.MapGet("/{id:int}", GetPrivateSiteById)
                .WithName(nameof(GetPrivateSiteById))
                .Produces<PrivateSiteResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Get Site By Id - Private")
                .WithDescription("Returns a single site and shows info that requires authorization.");

            privateGroup.MapPost("", CreateSite)
                .WithName(nameof(CreateSite))
                .Accepts<CreateSiteRequest>("application/json")
                .Produces<PrivateSiteResponse>(StatusCodes.Status201Created)
                .ProducesValidationProblem()
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Create a site")
                .WithDescription("Create a site. requires authentication.");

            privateGroup.MapPut("/{id:int}", UpdateSite)
                .WithName(nameof(UpdateSite))
                .Accepts<UpdateSiteRequest>("application/json")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .ProducesValidationProblem()
                .WithSummary("Update an existing site")
                .WithDescription("Update an existing site. requires authentication.");

            privateGroup.MapDelete("/{id:int}", DeleteSite)
                .WithName(nameof(DeleteSite))
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError)
                .WithSummary("Delete an existing site")
                .WithDescription("Delete an existing site. requires authentication."); 

            return route;
        }




        //Handlers for Sites

        private static async Task<Ok<List<PublicSiteResponse>>> GetAllPublicSites(ISiteService service, CancellationToken ct)
        {
            return TypedResults.Ok(await service.GetAllSitesPublicAsync(ct));
        }

        private static async Task<Results<Ok<PublicSiteResponse>, NotFound>> GetPublicSiteById(int Id,
            ISiteService service, CancellationToken ct)
        {
            var site = await service.GetPublicSiteByIdAsync(Id, ct);

            if (site is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(site);
        }


        private static async Task<Ok<List<PrivateSiteResponse>>> GetAllPrivateSites(ISiteService service, CancellationToken ct)
        {
            return TypedResults.Ok(await service.GetAllSitesPrivateAsync(ct));
        }

        private static async Task<Results<Ok<PrivateSiteResponse>, NotFound>> GetPrivateSiteById(int Id,
            ISiteService service, CancellationToken ct)
        {
            var site = await service.GetPrivateSiteByIdAsync(Id, ct);

            if (site is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(site);
        }

        private static async Task<Results<Created<PrivateSiteResponse>, ValidationProblem>> CreateSite(CreateSiteRequest request,
            ISiteService service,
            CancellationToken ct)
        {
            var createdSite = await service.CreateSiteAsync(request, ct);
            return TypedResults.Created($"/api/private/sites/{createdSite.Id}", createdSite);
        }

        private static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateSite (int Id,
            UpdateSiteRequest request, ISiteService service, CancellationToken ct)
        {
            var success = await service.UpdateSiteAsync(Id, request, ct);
            return success ? TypedResults.NoContent() : TypedResults.NotFound();
        }

        private static async Task<Results<NoContent, NotFound, ValidationProblem>> DeleteSite (int Id,
            ISiteService service, CancellationToken ct)
        {
            var success = await service.DeleteSiteAsync(Id, ct);
            return success ? TypedResults.NoContent() : TypedResults.NotFound();
        }

    }
}
