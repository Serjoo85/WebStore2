using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;
using WebStore.WebAPI.Clients.Base;

namespace WebStore.WebAPI.Clients.Employees;

public class EmployeesClient : BaseClient, IEmployeesData
{
    private readonly ILogger _logger;

    public EmployeesClient(HttpClient client, ILogger<EmployeesClient> logger) : base(client, WebApiAddresses.V1.Employees)
    {
        _logger = logger;
    }

    public IEnumerable<Employee> GetAll()
    {
        var employees = Get<IEnumerable<Employee>>(Address);
        return employees! ?? Enumerable.Empty<Employee>();
    }

    public Employee GetById(int id)
    {
        var employee = Get<Employee>($"{Address}/{id}");
        return employee!;
    }

    public int Add(Employee employee)
    {
        var response = Post(Address, employee);
        var addedEmployee = response.Content.ReadFromJsonAsync<Employee>().Result;
        if (addedEmployee is null)
            return -1;
        var id = addedEmployee!.Id;
        //TODO зачем? - мы сделали нового сотрудника и добавили его в БД, но в нашем списке со стороны приложения у него не бдует id ^^
        employee.Id = id;
        return id;
    }

    public bool Edit(Employee employee)
    {
        var response = Put(Address, employee);

        var success = response.EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<bool>()
            .Result;

        return success;
    }

    public bool Delete(int id)
    {
        var response = Delete($"{Address}/{id}");

        var success = response.EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<bool>()
            .Result;

        return success;
    }

    #region AsyncPart

    public async Task<IEnumerable<Employee >> GetAllAsync()
    {
        var employees = await GetAsync<IEnumerable<Employee>>(Address);
        return employees ?? Enumerable.Empty<Employee>();
    }

    public async Task<Employee?> GetByIdAsync(int id, CancellationToken token)
    {
        var employee = await GetAsync<Employee>($"{Address}/{id}", token);
        return employee;
    }

    public async Task<int> AddAsync(Employee employee, CancellationToken token)
    {
        var response = await PostAsync(Address, employee, token);
        var addedEmployee = response.Content.ReadFromJsonAsync<Employee>(cancellationToken: token).Result;
        if (addedEmployee is null)
            return -1;
        var id = addedEmployee.Id;
        employee.Id = id;
        return id;
    }

    public async Task<bool> EditAsync(Employee employee, CancellationToken token)
    {
        var response = await PutAsync(Address, employee, token);
        var success = response.EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<bool>(cancellationToken: token)
            .Result;
        return success;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken token)
    {
        var response = await DeleteAsync($"{Address}/{id}", token);
        var success = response.EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<bool>(cancellationToken: token)
            .Result;
        return success;
    } 
    #endregion
}