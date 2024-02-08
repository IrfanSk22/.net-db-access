using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DapperDemo.Models;
using DapperDemo.Repository;

namespace DapperDemo.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBonusRepository _bnsRepo;

    public HomeController(ILogger<HomeController> logger, IBonusRepository bnsRepo)
    {
        _logger = logger;
        _bnsRepo = bnsRepo;
    }
    public IActionResult Index()
    {
        IEnumerable<Company> companies = _bnsRepo.GetAllCompanyWithEmployees();
        return View(companies);
    }

    public ActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult AddTestRecords()
    {
        var company = new Company()
        {
            Name = "Test" +Guid.NewGuid().ToString(),
            Address = "test address",
            City = "test city",
            PostalCode = "test postalCode",
            State = "test state",
            Employees = new List<Employee>()
        };

        company.Employees.Add(new Employee()
        {
            Email = "test Email",
            Name = "Test Name " + Guid.NewGuid().ToString(),
            Phone = " test phone",
            Title = "Test Manager"
        });

        company.Employees.Add(new Employee()
        {
            Email = "test Email 2",
            Name = "Test Name 2" + Guid.NewGuid().ToString(),
            Phone = " test phone 2",
            Title = "Test Manager 2"
        });

        _bnsRepo.AddTestCompanyWithEmployeesWithTransaction(company);
        return RedirectToAction(nameof(Index));
    }
    
    public IActionResult RemoveTestRecords()
    {
        int[] companyIdToRemove = _bnsRepo.FilterCompanyByName("Test").Select(i => i.CompanyId).ToArray();
        _bnsRepo.RemoveRange(companyIdToRemove);
        return RedirectToAction(nameof(Index));
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
