using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeesData _employeesData;
    private readonly ILogger _logger;

    [HttpGet("api/employees/MyMethod/{id:int}")]
    public IActionResult MyRandomMethod(int id)
    {
        return Ok(777);
    }


    public EmployeesApiController(IEmployeesData employeesData, ILogger<EmployeesApiController> logger)
    {
        _employeesData = employeesData;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _employeesData.GetAll();
        if(employees.Any())
            return Ok(employees);
        return NoContent();
    }


    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var employee = _employeesData.GetById(id);
        if (employee is null)
            return NotFound();
        return Ok(employee);
    }
    
    [HttpPost]
    public IActionResult Add([FromBody] Employee employee)
    {
        //var id = await _employeesData.AddAsync(employee, CancellationToken.None);
        var id = _employeesData.Add(employee);
        _logger.LogInformation("Сотрудник {0} добавлен с идентификатором {1}", employee, id);

        return CreatedAtAction(nameof(GetById), new { id }, employee);
    }

    [HttpPut]
    public IActionResult Edit(Employee employee)
    {
        var success = _employeesData.Edit(employee);
         
        if (success)
        {
            _logger.LogInformation("Сотрудник {0} с идентификатором {1} отредактирован", employee, employee.Id);
            return Ok(true);
        }
        _logger.LogWarning("Ошибка. Сотрудник {0} с идентификатором {1} не отредактирован!", employee, employee.Id);
        return NotFound(false);
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var success = _employeesData.Delete(id);
        if (success)
        {
            _logger.LogInformation("Сотрудник с идентификатором {0} удалён", id);
            return Ok(true);
        }
        _logger.LogWarning("Ошибка. Сотрудник с идентификатором {0} не удалён!", id);
        return NotFound(false);
    }
}