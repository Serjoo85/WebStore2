using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services;


public interface IEmployeesData
{
    //TODO не нужно возвращать bool при редактировании и удалении.
    IEnumerable<Employee> GetAll();
    Task<Employee?> GetByIdAsync(int id, CancellationToken token);
    Task<int> AddAsync(Employee employee, CancellationToken token);
    Task<bool> EditAsync(Employee employee, CancellationToken token);
    Task<bool> DeleteAsync(int id, CancellationToken token);

    Employee GetById(int id);
    int Add(Employee employee);
    bool Edit(Employee employee);
    bool Delete(int id);
}