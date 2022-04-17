using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeesData _employeesData;
    private readonly ILogger _logger;

    public EmployeesApiController(IEmployeesData employeesData, ILogger logger)
    {
        _employeesData = employeesData;
        _logger = logger;
    }


}