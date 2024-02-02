using DapperDemo.Data;
using DapperDemo.Models;

namespace DapperDemo.Repository;

public class CompanyRepositoryEf : ICompanyRepository
{
    private readonly ApplicationDbContext _context;

    public CompanyRepositoryEf(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<Company> GetAll()
    {
        return _context.Company.ToList();
    }
    
    public Company Find(int id)
    {
        return _context.Company.FirstOrDefault(u => u.CompanyId==id);
    }

    public Company Add(Company company)
    {
        _context.Company.Add(company);
        _context.SaveChanges();

        return company;
    }

    public Company Update(Company company)
    {
        _context.Company.Update(company);
        _context.SaveChanges();

        return company;
    }

    public void Remove(int id)
    {
        var company = _context.Company.FirstOrDefault(c => c.CompanyId == id);
        if (company != null)
        {
            _context.Company.Remove(company);
            _context.SaveChanges();
        }
    }
}
