
using AeonRegistryAPI.Endpoints.Home;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddCustomSwagger();

var app = builder.Build();

// Enable Swagger
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseStaticFiles();


app.MapHomeEndpoints();

app.Run();

