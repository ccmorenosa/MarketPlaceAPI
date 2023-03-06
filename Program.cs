/* Program.cs
 *
 * Main source that build anr run the API.
 */

using Microsoft.EntityFrameworkCore;
using MarketPlaceAPI.Models;

// Create the app builder with the arguments.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<MarketContext>(
    opt => opt.UseInMemoryDatabase("Market")
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Create the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
