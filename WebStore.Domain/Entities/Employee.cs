using WebStore.Domain.Entities.Base;

namespace WebStore.Domain.Entities;

public class Employee : Entity
{
    public string LastName { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string Patronymic { get; set; }

    public int Age { get; set;}

    public string Position { get; set; }

    public decimal Salary { get; set; }
}