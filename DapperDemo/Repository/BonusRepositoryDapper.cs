using System.Data;
using System.Transactions;
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

    public void AddTestCompanyWithEmployees(Company objComp)
    {
        var sql = "INSERT INTO Company (Name, Address, City, State, PostalCode) " +
                  "VALUES(@Name, @Address, @City, @State, @PostalCode);" +
                  "SELECT CAST(SCOPE_IDENTITY() as int);";

        var id = _db.Query<int>(sql, objComp).Single();
        objComp.CompanyId = id;

        /*
        foreach (var employee in objComp.Employees)
        {
            employee.CompanyId = objComp.CompanyId;
            var sql1 =
                "INSERT INTO     Employees (Name, Title, Email, Phone, CompanyId) " +
                "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
                "SELECT CAST(SCOPE_IDENTITY() as int);";
            
            _db.Query<int>(sql1, employee).Single();
        }
        */
        
        // batch insert
        objComp.Employees.Select(c => 
        { 
            c.CompanyId = id;
            return c;
        }).ToList();
        
        const string sqlEmp = "INSERT INTO     Employees (Name, Title, Email, Phone, CompanyId) " +
                     "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
                     "SELECT CAST(SCOPE_IDENTITY() as int);";

        _db.Execute(sqlEmp, objComp.Employees);
    }
    
    public void AddTestCompanyWithEmployeesWithTransaction(Company objComp)
    {
        using (var transaction = new TransactionScope())
        {
            try
            {
                var sql = "INSERT INTO Company (Name, Address, City, State, PostalCode) " +
                          "VALUES(@Name, @Address, @City, @State, @PostalCode);" +
                          "SELECT CAST(SCOPE_IDENTITY() as int);";

                var id = _db.Query<int>(sql, objComp).Single();
                objComp.CompanyId = id;
        
                // batch insert
                objComp.Employees.Select(c => 
                { 
                    c.CompanyId = id;
                    return c;
                }).ToList();
        
                const string sqlEmp = "INSERT INTO     Employees (Name, Title, Email, Phone, CompanyId) " +
                                      "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
                                      "SELECT CAST(SCOPE_IDENTITY() as int);";
                
                _db.Execute(sqlEmp, objComp.Employees);
                transaction.Complete();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }

    public void RemoveRange(int[] companyId)
    {
        const string sql = "DELETE FROM Company WHERE CompanyId IN @companyId";
        _db.Query(sql, new { companyId });
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
    
    public List<Company> FilterCompanyByName(string name)
    {
        const string sql = "SELECT * FROM Company WHERE Name LIKE '%' + @name + '%' ";
        return _db.Query<Company>(sql, new { name }).ToList();
    }
}
