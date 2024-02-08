using System.Data;
using Microsoft.Data.SqlClient;
using DapperDemo.Models;

namespace DapperDemo.Repository;

public class CompanyRepositoryAdoNet : ICompanyRepository
{
    private SqlConnection _conn;

    public CompanyRepositoryAdoNet(IConfiguration configuration)
    {
        _conn = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
    }

    public Company Add(Company company)
    {
        using (var cmd = new SqlCommand("usp_AddCompany", _conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", company.Name);
            cmd.Parameters.AddWithValue("@Address", company.Address);
            cmd.Parameters.AddWithValue("@City", company.City);
            cmd.Parameters.AddWithValue("@State", company.State);
            cmd.Parameters.AddWithValue("@PostalCode", company.PostalCode);

            // Output parameter for CompanyId
            var companyIdParameter = new SqlParameter
            {
                ParameterName = "@CompanyId",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            cmd.Parameters.Add(companyIdParameter);

            _conn.Open();
            cmd.ExecuteNonQuery();

            // Retrieve the output parameter value
            company.CompanyId = (int)companyIdParameter.Value;
            _conn.Close();
        }

        return company;
    }

    public List<Company> GetAll()
    {
        List<Company> compList = new List<Company>();

        using (var cmd = new SqlCommand("usp_GetALLCompany", _conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            // Bind EmpModel generic list using dataRow
            foreach (DataRow dr in dt.Rows)
            {
                compList.Add(new Company
                {
                    CompanyId = Convert.ToInt32(dr["CompanyId"]),
                    Name = Convert.ToString(dr["Name"]),
                    Address = Convert.ToString(dr["Address"]),
                    City = Convert.ToString(dr["City"]),
                    State = Convert.ToString(dr["State"]),
                    PostalCode = Convert.ToString(dr["PostalCode"])
                });
            }
        }

        return compList;
    }

    public Company Find(int id)
    {
        Company company = null;

        using (var cmd = new SqlCommand("usp_GetCompany", _conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyId", id);

            var da = new SqlDataAdapter(cmd);
            var dt = new DataTable();

            _conn.Open();
            da.Fill(dt);
            _conn.Close();

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                company = new Company
                {
                    CompanyId = Convert.ToInt32(dr["CompanyId"]),
                    Name = Convert.ToString(dr["Name"]),
                    Address = Convert.ToString(dr["Address"]),
                    City = Convert.ToString(dr["City"]),
                    State = Convert.ToString(dr["State"]),
                    PostalCode = Convert.ToString(dr["PostalCode"])
                };
            }
        }

        return company;
    }

    public Company Update(Company company)
    {
        using (var cmd = new SqlCommand("usp_UpdateCompany", _conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyId", company.CompanyId);
            cmd.Parameters.AddWithValue("@Address", company.Address);
            cmd.Parameters.AddWithValue("@Name", company.Name);
            cmd.Parameters.AddWithValue("@City", company.City);
            cmd.Parameters.AddWithValue("@State", company.State);
            cmd.Parameters.AddWithValue("@PostalCode", company.PostalCode);

            _conn.Open();
            cmd.ExecuteNonQuery();
            _conn.Close();
        }
        return company;
    }

    public void Remove(int id)
    {
        using (var cmd = new SqlCommand("usp_RemoveCompany", _conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CompanyId", id);

            _conn.Open();
            cmd.ExecuteNonQuery();
            _conn.Close();
        }
    }
}
