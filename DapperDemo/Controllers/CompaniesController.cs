using Microsoft.AspNetCore.Mvc;
using DapperDemo.Models;
using DapperDemo.Repository;
using DapperDemo.Repository.IRepository;

namespace DapperDemo.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ICompanyRepository _compRepo;
        private readonly IEmployeeRepository _empRepo;
        private readonly IBonusRepository _bnsRepo;
        private readonly IDapperSprocRepo _sprocRepo;

        public CompaniesController(ICompanyRepository compRepo,
            IEmployeeRepository empRepo,
            IBonusRepository bnsRepo,
            IDapperSprocRepo sprocRepo)
        {
            _compRepo = compRepo;
            _empRepo = empRepo;
            _bnsRepo = bnsRepo;
            _sprocRepo = sprocRepo;
        }

        // GET: Companies
        public IActionResult Index()
        {
            return View(_compRepo.GetAll());
            // return View(_sprocRepo.List<Company>("usp_GetALLCompany"));
        }

        // GET: Companies/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = _bnsRepo.GetCompanyWithEmployees(id.GetValueOrDefault());
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (ModelState.IsValid)
            {
                _compRepo.Add(company);
                return RedirectToAction(nameof(Index));
            }

            // Log or debug ModelState errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                // Log or debug the error messages
                Console.WriteLine(error.ErrorMessage);
            }

            return View(company);
        }

        // GET: Companies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var company = _sprocRepo.Single<Company>("usp_GetCompany", new { CompanyId = id.GetValueOrDefault()});
            var company = _compRepo.Find(id.GetValueOrDefault());

            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CompanyId,Name,Address,City,State,PostalCode")] Company company)
        {
            if (id != company.CompanyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _compRepo.Update(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _compRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }
    }
}
