using System.Data;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;

namespace DapperDemo.Repository;

public class CompanyRepositoryDapper : ICompanyRepository
{
    private IDbConnection _db;
    
    public CompanyRepositoryDapper(IConfiguration configuration)
    {
        _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public List<Company> GetAll()
    {
        var sql = "SELECT * FROM Company;";
        
        return _db.Query<Company>(sql).ToList();
    }
    
    public Company Find(int id)
    {
        var sql = "SELECT * FROM Company WHERE CompanyId = @CompanyId;";
        
        return _db.Query<Company>(sql, new { @CompanyId = id }).Single();
    }

    public Company Add(Company company)
    {
        var sql =
            "INSERT INTO Company (Name, Address, City, State, PostalCode) " + 
            "VALUES(@Name, @Address, @City, @State, @PostalCode);" + 
            "SELECT CAST(SCOPE_IDENTITY() as int);";
        
        var id = _db.Query<int>(sql, company).Single();
        company.CompanyId = id;
        
        return company;
    }

    public Company Update(Company company)
    {
        var sql = "UPDATE Company SET Name = @Name, Address = @Address, City = @City, " +
                  "State = @State, PostalCode = @PostalCode WHERE CompanyId = @CompanyId";

        _db.Execute(sql, company);
        return company;
    }

    public void Remove(int id)
    {
        var sql = "DELETE FROM Company WHERE CompanyId = @CompanyId";

        _db.Execute(sql, new { @CompanyId = id });
    }
}
