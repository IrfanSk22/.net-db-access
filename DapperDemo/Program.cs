using DapperDemo.Data;
using DapperDemo.Repository;
using DapperDemo.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        builder.Services.AddScoped<IEmployeeRepository, EmployeeRepositoryDapper>();
        builder.Services.AddScoped<ICompanyRepository, CompanyRepositoryAdoNet>();
        // builder.Services.AddScoped<ICompanyRepository,CompanyRepositoryDapperSp>();
        // builder.Services.AddScoped<ICompanyRepository,CompanyRepositoryDapperContrib>();
        builder.Services.AddScoped<IBonusRepository, BonusRepositoryDapper>();
        builder.Services.AddScoped<IDapperSprocRepo, DapperSprocRepo>();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

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
    }
}
