using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL;

public class SqlEmployeeData : IEmployeesData
{
    public SqlEmployeeData()
    {
        
    }

    public IEnumerable<Employee> GetAll()
    {
        throw new NotImplementedException();
    }

    public Employee GetById(int id)
    {
        throw new NotImplementedException();
    }

    public int Add(Employee employee)
    {
        throw new NotImplementedException();
    }

    public bool Edit(Employee employee)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }
}