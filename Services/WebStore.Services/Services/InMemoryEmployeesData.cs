using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Data;

namespace WebStore.Services.Services;

public class InMemoryEmployeesData : IEmployeesData
{
    private int _lastFreeId;
    private readonly ILogger<InMemoryEmployeesData> _Logger;
    private readonly ICollection<Employee> _employees;
    
    public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> logger)
    {
        _employees = TestData.__Employees;
        _Logger = logger;
        _lastFreeId = _employees.Count == 0
            ? 1
            : _employees.Max(emp => emp.Id);
        _lastFreeId++;
    }

    public IEnumerable<Employee> GetAll() => _employees;


    public Task<int> Add(Employee employee, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task Edit(Employee employee, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task<Employee> GetById(int id, CancellationToken token)
    {
        var emp = _employees.FirstOrDefault(emp => emp.Id == id);
        return emp;
    }


    public async Task<int> Add(Employee employee)
    {
        if(employee is null)
            throw new ArgumentNullException(nameof(employee));
        
        employee.Id = _lastFreeId++
            ;
        _employees.Add(employee);
        _Logger.LogWarning("Сотрудник добавлен id{0}", employee.Id);
        return employee.Id;
    }

    public async Task<bool> Edit(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));
        if (_employees.Contains(employee))
            return true;
    
        var db_emp = await GetById(employee.Id, CancellationToken.None);
        if (db_emp is null)
        {
            _Logger.LogWarning("Попытка редактирования несуществуюющего сотрудника id{0}", db_emp.Id);
            return false;
        }

        db_emp.LastName = employee.LastName;
        db_emp.FirstName = employee.FirstName;
        db_emp.Patronymic = employee.Patronymic;
        db_emp.Age = employee.Age;
        db_emp.Position = employee.Position;
        db_emp.Salary = employee.Salary;

        _Logger.LogWarning("Сотрудник отредактирован id{0}", db_emp.Id);
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var db_emp = await GetById(id, CancellationToken.None);
        if (db_emp is null)
        {
            _Logger.LogWarning("Попытка удалить отсутствующего сотрудника Id {0}", id);
            return false;
        }
        _employees.Remove(db_emp);
        return true;
    }
}