using System.Data;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;

namespace DapperDemo.Repository;

public class BonusRepositoryDapper : IBonusRepository
{
    private IDbConnection _db;
    
    public BonusRepositoryDapper(IConfiguration configuration)
    {
        _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }
    
    public List<Company> GetAllCompanyWithEmployees()
    {
        var sql = "SELECT C.*, E.* FROM Employees AS E INNER JOIN Company AS C ON E.CompanyId = C.CompanyId";

        var companyDict = new Dictionary<int, Company>();

        var company = _db.Query<Company, Employee, Company>(sql, (c, e) =>
        {
            if (!companyDict.TryGetValue(c.CompanyId, out var currentCompany))
            {
                currentCompany = c;
                companyDict.Add(currentCompany.CompanyId, currentCompany);
            }
            currentCompany.Employees.Add(e);
            return currentCompany;
            
        }, splitOn: "EmployeeId");

        return company.Distinct().ToList();
    }

    public Company GetCompanyWithEmployees(int id)
    {
        Company company;
        
        var p = new
        {
            CompanyId = id
        };
        
        var sql = "SELECT * FROM Company WHERE CompanyId = @CompanyId;" + 
                  " SElECT * FROM Employees WHERE CompanyId = @CompanyId;";

        using (var lists = _db.QueryMultiple(sql, p))
        {
            company = lists.Read<Company>().ToList().FirstOrDefault();
            company.Employees = lists.Read<Employee>().ToList();
        }

        return company;
    }

    public List<Employee> GetEmployeeWithCompany(int id)
    {
        var sql = "SELECT E.*, C.* FROM Employees AS E INNER JOIN Company AS C ON E.CompanyId = C.CompanyId";
        if (id != 0)
        {
            sql += " WHERE E.CompanyId = @Id";
        }
   
        var employee = _db.Query<Employee, Company, Employee>(sql, (e, c) =>
        {
            e.Company = c;
            return e;
        }, new { ID = id }, splitOn: "CompanyId");
   
        return employee.ToList();
    }
}
