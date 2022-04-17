using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees;

public class EmployeesClient : BaseClient, IEmployeesData
{
    private readonly ILogger _logger;

    public EmployeesClient(HttpClient client, ILogger logger) : base(client, "api/employees")
    {
        _logger = logger;
    }

    public IEnumerable<Employee> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetById(int id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

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
}