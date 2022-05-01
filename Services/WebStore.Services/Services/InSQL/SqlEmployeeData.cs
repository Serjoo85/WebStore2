using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Services.InSQL;

public class SqlEmployeeData : IEmployeesData
{
    private readonly WebStoreDb _db;
    private readonly ILogger _logger;

    public SqlEmployeeData(WebStoreDb db, ILogger<SqlEmployeeData> logger)
    {
        _db = db;
        _logger = logger;
    }

    public IEnumerable<Employee> GetAll() => _db.Employees;

    public Task<IEnumerable<Employee>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Employee> GetByIdAsync(int id, CancellationToken token = default)
    { 
        return await _db.Employees.FirstOrDefaultAsync(emp => emp.Id == id, token).ConfigureAwait(false);
    }

    public Employee GetById(int id) => GetByIdAsync(id).Result;

    public int Add(Employee employee) => AddAsync(employee).Result;

    public async Task<int> AddAsync(Employee employee, CancellationToken token = default)
    {
        _logger.LogInformation("Добавление нового сотрудника ...");
        await _db.Employees.AddAsync(employee, token).ConfigureAwait(false);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
        _logger.LogInformation("Cотрудник {0} успешно добавлен.", employee.LastName);
        return employee.Id;
    }

    public bool Edit(Employee employee) => EditAsync(employee).Result;

    public async Task<bool> EditAsync(Employee employee, CancellationToken token = default)
    {
        _logger.LogInformation("Редактирование сотрудника ...");
        _db.Employees.Update(employee);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
        _logger.LogInformation("Сотрудник {0} успешно отредактирован.", employee.LastName);
        //TODO обернуть в трай кетч.
        return true;
    }

    public bool Delete(int id) => DeleteAsync(id).Result;
    public async Task<bool> DeleteAsync(int id, CancellationToken token = default)
    {
        _logger.LogInformation("Удаление сотрудника ...");
        var emp = await GetByIdAsync(id, token);
        _db.Employees.Remove(emp);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
        _logger.LogInformation("Сотрудник {0} удалён.", emp.LastName);
        //TODO переделать черзе трай кетч.
        return true;
    }
}