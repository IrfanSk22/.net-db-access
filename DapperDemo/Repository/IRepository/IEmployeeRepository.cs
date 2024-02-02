using DapperDemo.Models;

namespace DapperDemo.Repository;

public interface IEmployeeRepository
{
    List<Employee> GetAll();
    
    Employee Find(int id);

    Employee Add(Employee employee);

    Employee Update(Employee employee);

    void Remove(int id);
}
