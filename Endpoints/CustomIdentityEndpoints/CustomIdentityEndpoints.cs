using AeonRegistryAPI.Endpoints.CustomIdentityEndpoints.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace AeonRegistryAPI.Endpoints.CustomIdentityEndpoints
{
    public static class CustomIdentityEndpoints
    {

        public static IEndpointRouteBuilder MapCustomIdentityEndpoints(this IEndpointRouteBuilder route)
        {
            //step 1 - Make a group
            var group = route.MapGroup("/api/auth")
                .WithTags("Admin");

            //step 2 - Make an endpoint
            group.MapPost("/register-admin", RegisterUser)
                .WithName("RegisterAdmin")
                .WithSummary("Register a User")
                .WithDescription("Registers a user. User must have admin role.");
            //.RequireAuthorization("AdminPolicy");

            //step 3 - implement route handler

            //step 4 - return route
            return route;
        }

        private static async Task<IResult> RegisterUser(RegisterUserRequest dto, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IConfiguration config)
        {
            //see if useremail sent in already exists
            if (await userManager.FindByEmailAsync(dto.Email) is not null)
            {
                return Results.BadRequest(new { Error = $"User with email {dto.Email} already exists." });
            }

            var user = new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };

            var tempPassword = "TempPassword123!"; //Meet the password requirements  (generate in real world).

            var created = await userManager.CreateAsync(user, tempPassword);

            if (!created.Succeeded)
            {
                return Results.BadRequest(new { Error = created.Errors });
            }

            if (await roleManager.RoleExistsAsync("Researcher"))
            {
                await userManager.AddToRoleAsync(user, "Researcher");
            }

            //generate password reset token
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            // send email to user to change password
            var baseUrl = config["BaseURL"] ?? "https://localhost:7008";

            await emailSender.SendEmailAsync(
                dto.Email!,
                "Welcome to the Aeon Registry",
                $"""
                Your account has been created. Please change your password by visiting: {baseUrl}/Setpassword.html

                {baseUrl}/Setpassword.html?email={dto.Email}&resetCode={encodedToken}

                """
                );

            return Results.Ok(new {Message = $"User {user.Email} created. Password reset link sent."});
        }

    }
}
