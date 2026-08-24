using IronGridApi.Models;
using IronGridApi.Data;
using IronGridApi.Repository;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("testDb");
builder.Services.AddDbContext<IronGridDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
); 
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<ICriticalAssetRepository, CriticalAssetRepository>();
builder.Services.AddScoped<IAssetsStatusRepository, AssetsStatusRepository>();


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
