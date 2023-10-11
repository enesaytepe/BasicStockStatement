using Common;
using DataAccess.Interfaces;
using DataAccess.Repositories.Ado;
using DataAccess.Repositories.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

string repositoryType = configuration["RepositoryType"];

if (repositoryType == "Ef")
{
    builder.Services.AddScoped<IStockRepository, EfStockRepository>();
}
else if (repositoryType == "AdoNet")
{
    builder.Services.AddScoped<IStockRepository, AdoNetStockRepository>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
