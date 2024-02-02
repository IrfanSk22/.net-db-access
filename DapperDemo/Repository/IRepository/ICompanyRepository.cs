using DapperDemo.Models;

namespace DapperDemo.Repository;

public interface ICompanyRepository
{
    List<Company> GetAll();
    
    Company Find(int id);

    Company Add(Company company);

    Company Update(Company company);

    void Remove(int id);
}
