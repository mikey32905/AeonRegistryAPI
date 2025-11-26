
using AeonRegistryAPI.Data;
using AeonRegistryAPI.Endpoints.Home;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

///Build Section of API
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCustomSwagger();

//get a connection to the database
var connectionString = DataUtility.GetConnectionString(builder.Configuration);

//configure for database context for PostgresSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

//add in Identity endpoints
builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options =>{
    options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Admin Policy
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

//enable validation for minimal APIs
builder.Services.AddValidation();

///App Section of API
var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<BlockIdentityEndpoints>();

var authRouteGroup = app.MapGroup("/api/auth")
    .WithTags("Admin");
authRouteGroup.MapIdentityApi<ApplicationUser>();

app.MapHomeEndpoints();

app.Run();

