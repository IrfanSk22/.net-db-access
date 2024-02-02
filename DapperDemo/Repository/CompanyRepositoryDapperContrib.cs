using System.Data;
using Dapper;
using Dapper.Contrib.Extensions;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;

namespace DapperDemo.Repository;

public class CompanyRepositoryDapperContrib : ICompanyRepository
{
    private IDbConnection _db;
    
    public CompanyRepositoryDapperContrib(IConfiguration configuration)
    {
        _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public List<Company> GetAll()
    {
        return _db.GetAll<Company>().ToList();
    }
    
    public Company Find(int id)
    {
        return _db.Get<Company>(id);
    }

    public Company Add(Company company)
    {
        var id = _db.Insert(company);
        company.CompanyId = (int)id;
        return company;
    }

    public Company Update(Company company)
    {
        _db.Update(company);
        return company;
    }

    public void Remove(int id)
    {
        _db.Delete(new Company { CompanyId = id});
    }
}
