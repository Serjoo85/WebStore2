using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL;

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

    public async Task Add(Employee employee, CancellationToken token = default)
    {
        await _db.Employees.AddAsync(employee, token).ConfigureAwait(false);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task Edit(Employee employee, CancellationToken token = default)
    {
        _db.Employees.Update(employee);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
    }

    public async Task Delete(int id, CancellationToken token = default)
    {
        var emp = GetById(id, token);
        _db.Employees.Remove(await emp);
        await _db.SaveChangesAsync(token).ConfigureAwait(false);
    }
}