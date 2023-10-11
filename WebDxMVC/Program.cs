using Common;
using DataAccess.Interfaces;
using DataAccess.Repositories.Ado;
using DataAccess.Repositories.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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

builder.Services.AddDbContext<TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
