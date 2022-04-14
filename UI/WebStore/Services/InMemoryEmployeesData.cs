using WebStore.Data;
using WebStore.Domain.Entities;

namespace WebStore.Services;

public class InMemoryEmployeesData
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

    public Employee? GetById(int id)
    {
        var emp = _employees.FirstOrDefault(emp => emp.Id == id);
        return emp;
    }


    public void Add(Employee employee)
    {
        if(employee is null)
            throw new ArgumentNullException(nameof(employee));
        
        employee.Id = _lastFreeId++
            ;
        _employees.Add(employee);
        _Logger.LogWarning("Сотрудник добавлен id{0}", employee.Id);
    }

    public bool Edit(Employee employee)
    {
        if (employee is null)
            throw new ArgumentNullException(nameof(employee));
        if (_employees.Contains(employee))
            return true;
    
        var db_emp = GetById(employee.Id);
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

    public bool Delete(int id)
    {
        var db_emp = GetById(id);
        if (db_emp is null)
        {
            _Logger.LogWarning("Попытка удалить отсутствующего сотрудника Id {0}", id);
            return false;
        }
        _employees.Remove(db_emp);
        return true;
    }
}