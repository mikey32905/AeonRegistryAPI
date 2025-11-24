
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


app.MapGet("/api/welcome", () =>
{
    var response = new
    {
        Message = "Welcome to the Aeon Registry API",
        Version = "1.0.0",
        TimeOnly = DateTime.Now.ToString("T")
    };

    return Results.Ok(response);
 })   .WithName("Welcome Message");

app.Run();

