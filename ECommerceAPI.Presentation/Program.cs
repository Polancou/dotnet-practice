using ECommerceAPI.Application;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Presentation.Middleware;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add Domain, Application and Infrastructure via Extensions
builder.Services.AddApplication();
builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\mssqllocaldb;Database=ECommerceAPI;Trusted_Connection=True;MultipleActiveResultSets=true");

builder.Services.AddOpenApi();

// Add Global Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.WithTitle("E-Commerce Practice API");
        options.WithTheme(ScalarTheme.DeepSpace);
    });
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
