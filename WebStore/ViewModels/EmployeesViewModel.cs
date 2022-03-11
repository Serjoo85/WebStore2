namespace WebStore.ViewModels;

public class EmployeesViewModel
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string Patronymic { get; set; }

    public int Age { get; set; }

    public string Position { get; set; }

    public decimal Salary { get; set; }
}