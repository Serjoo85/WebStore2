using WebStore.Models;

namespace WebStore.Data;

public class TestData
{
    public static readonly List<Employee> __Employees { get; } = new List<Employee>
    {
        new Employee
        {
            Id = 1, Age = 21, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович",
            Position = "Менеджер читсоты", Salary = 10000
        },
        new Employee
        {
            Id = 2, Age = 22, LastName = "Пётров", FirstName = "Пётр", Patronymic = "Петрович",
            Position = "Менеджер читсоты", Salary = 10000
        },
        new Employee
        {
            Id = 3, Age = 23, LastName = "Сидоров", FirstName = "Сидр", Patronymic = "Сидорович",
            Position = "Менеджер читсоты", Salary = 10000
        },
    };
}