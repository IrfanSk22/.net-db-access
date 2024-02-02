using Microsoft.AspNetCore.Mvc;
using DapperDemo.Models;
using DapperDemo.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DapperDemo.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ICompanyRepository _compRepo;
        private readonly IEmployeeRepository _empRepo;
        private readonly IBonusRepository _bnsRepo;

        public EmployeesController(ICompanyRepository compRepo, IEmployeeRepository empRepo,
            IBonusRepository bnsRepo)
        {
            _compRepo = compRepo;
            _empRepo = empRepo;
            _bnsRepo = bnsRepo;
        }

        // GET: Employees
        public IActionResult Index(int companyId = 0)
        {
            /*
            // N + 1 Call
            // not optimal as for each one of the employees its making a separate database calls.
            List<Employee> employees = _empRepo.GetAll();
            foreach (Employee obj in employees)
            {
                obj.Company = _compRepo.Find(obj.CompanyId);
            }
            */
            var employees = _bnsRepo.GetEmployeeWithCompany(companyId);

            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });

            ViewBag.CompanyList = companyList;
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EmployeeId, Name, Email, Phone, Title, CompanyId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _empRepo.Add(employee);
                return RedirectToAction(nameof(Index));
            }

            // Log or debug ModelState errors
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                // Log or debug the error messages
                Console.WriteLine(error.ErrorMessage);
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _empRepo.Find(id.GetValueOrDefault());

            IEnumerable<SelectListItem> companyList = _compRepo.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.CompanyId.ToString()
            });

            ViewBag.CompanyList = companyList;

            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("EmployeeId, Name, Email, Phone, Title, CompanyId")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _empRepo.Update(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _empRepo.Remove(id.GetValueOrDefault());
            return RedirectToAction(nameof(Index));
        }
    }
}
