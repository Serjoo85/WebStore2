using WebStore.Domain.Entities;

namespace WebStore.Services.Interfaces;

public interface IEmployeesData
{
    IEnumerable<Employee> GetAll();

    Task<Employee> GetById(int id, CancellationToken token);
    Task Add(Employee employee, CancellationToken token);
    Task Edit(Employee employee, CancellationToken token);
    Task Delete(int id, CancellationToken token);
}