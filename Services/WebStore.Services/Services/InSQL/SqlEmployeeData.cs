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

    public async Task<Employee> GetById(int id, CancellationToken token = default)
    { 
        return await _db.Employees.FirstOrDefaultAsync(emp => emp.Id == id, token).ConfigureAwait(false);
    }

    public async Task<int> Add(Employee employee, CancellationToken token = default)
    {
        _logger.LogInformation("Добавление нового сотрудника ...");
        await _db.Employees.AddAsync(employee, token).ConfigureAwait(false);
        var x = await _db.SaveChangesAsync(token).ConfigureAwait(false);
        _logger.LogInformation("Cотрудник {0} успешно добавлен.", employee.LastName);
        return x;
    }

    public async Task Edit(Employee employee, CancellationToken token = default)
    {
        _logger.LogInformation("Редактирование сотрудника ...");
        _db.Employees.Update(employee);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
        _logger.LogInformation("Сотрудник {0} успешно отредактирован.", employee.LastName);
    }

    public async Task Delete(int id, CancellationToken token = default)
    {
        _logger.LogInformation("Удаление сотрудника ...");
        var emp = await GetById(id, token);
        _db.Employees.Remove(emp);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
        _logger.LogInformation("Сотрудник {0} удалён.", emp.LastName);
    }
}