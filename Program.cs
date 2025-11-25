
using AeonRegistryAPI.Data;
using AeonRegistryAPI.Endpoints.Home;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCustomSwagger();

//get a connection to the database
var connectionString = DataUtility.GetConnectionString(builder.Configuration);

//configure for database context for PostgresSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseStaticFiles();


app.MapHomeEndpoints();

app.Run();

