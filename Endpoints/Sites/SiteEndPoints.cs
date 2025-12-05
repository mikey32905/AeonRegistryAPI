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
            var publicGroup = route.MapGroup("/api/sites")
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

    }
}
