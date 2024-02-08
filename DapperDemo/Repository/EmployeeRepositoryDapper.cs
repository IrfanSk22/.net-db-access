using System.Data;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;

namespace DapperDemo.Repository;

public class EmployeeRepositoryDapper : IEmployeeRepository
{
    private IDbConnection _db;
    
    public EmployeeRepositoryDapper(IConfiguration configuration)
    {
        _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public List<Employee> GetAll()
    {
        var sql = "SELECT * FROM Employees;";
        
        return _db.Query<Employee>(sql).ToList();
    }
    
    public Employee Find(int id)
    {
        var sql = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId;";
        
        return _db.Query<Employee>(sql, new { @EmployeeId = id }).Single();
    }

    public Employee Add(Employee employee)
    {
        var sql =
            "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) " +
            "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
            "SELECT CAST(SCOPE_IDENTITY() as int);";
        
        var id = _db.Query<int>(sql, employee).Single();
        employee.EmployeeId = id;
        return employee;
    }
    
    public async Task<Employee> AddAsync(Employee employee)
    {
        var sql =
            "INSERT INTO Employees (Name, Title, Email, Phone, CompanyId) " +
            "VALUES(@Name, @Title, @Email, @Phone, @CompanyId);" +
            "SELECT CAST(SCOPE_IDENTITY() as int);";
        
        var id = await _db.QueryAsync<int>(sql, employee);
        employee.EmployeeId = id.Single();
        return employee;
    }

    public Employee Update(Employee employee)
    {
        var sql = "UPDATE Employees SET Name = @Name, Title = @Title, Email = @Email, " +
                  "Phone = @Phone, CompanyId = @CompanyId WHERE EmployeeId = @EmployeeId";

        _db.Execute(sql, employee);
        return employee;
    }

    public void Remove(int id)
    {
        var sql = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
        _db.Execute(sql, new { @EmployeeId = id });
    }
}
