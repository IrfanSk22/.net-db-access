using System.Data;
using Dapper;
using DapperDemo.Models;
using Microsoft.Data.SqlClient;

namespace DapperDemo.Repository;

public class CompanyRepositoryDapperSp : ICompanyRepository
{
    private IDbConnection _db;
    
    public CompanyRepositoryDapperSp(IConfiguration configuration)
    {
        _db = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public List<Company> GetAll()
    {
        return _db.Query<Company>("usp_GetALLCompany", commandType: CommandType.StoredProcedure).ToList();
    }
    
    public Company Find(int id)
    {
        return _db.Query<Company>("usp_GetCompany", new { @CompanyId = id }, commandType: CommandType.StoredProcedure).Single();
    }

    public Company Add(Company company)
    {
        var parameters = new DynamicParameters();
        parameters.Add("CompanyId", 0, DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@Name", company.Name);
        parameters.Add("@Address", company.Address);  
        parameters.Add("@City", company.City);
        parameters.Add("@State", company.State);
        parameters.Add("@PostalCode", company.PostalCode);

        _db.Execute("usp_AddCompany", parameters, commandType: CommandType.StoredProcedure);
        company.CompanyId = parameters.Get<int>("CompanyId");

        return company;
    }

    public Company Update(Company company)
    {
        // _db.Execute("usp_UpdateCompany", company, commandType: CommandType.StoredProcedure);
        /*
       _db.Execute("usp_UpdateCompany", new
       {
           @CompanyId = company.CompanyId,
           @Name = company.Name,
           @Address = company.Address,
           @City = company.City,
           @State = company.State,
           @PostalCode = company.PostalCode
       }, commandType: CommandType.StoredProcedure);
       */
        // having more controls over your parameters
        var parameters = new DynamicParameters();
        parameters.Add("CompanyId", company.CompanyId);
        parameters.Add("@Name", company.Name);
        parameters.Add("@Address", company.Address);
        parameters.Add("@City", company.City);
        parameters.Add("@State", company.State);
        parameters.Add("@PostalCode", company.PostalCode);

        _db.Execute("usp_UpdateCompany", parameters, commandType: CommandType.StoredProcedure);
        
        return company;
    }

    public void Remove(int id)
    {
        _db.Execute("usp_RemoveCompany", new { @CompanyId = id }, commandType: CommandType.StoredProcedure);
    }
}
