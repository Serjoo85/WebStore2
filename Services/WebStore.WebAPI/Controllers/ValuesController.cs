using Microsoft.AspNetCore.Mvc;

namespace WebStore.WebAPI.Controllers;

//[Route("api/[controller]")]
[Route("api/values")]
[ApiController]
public class ValuesController : ControllerBase
{
    private readonly ILogger<ValuesController> _logger;

    private static readonly Dictionary<int, string> Values = Enumerable.Range(1, 10)
        .Select(i => (Id: i, Value: $"Value-{i}"))
        .ToDictionary(v => v.Id, v => v.Value);

    public ValuesController(ILogger<ValuesController> logger)
    {
        _logger = logger;
    }

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        var values = Values.Values;
        return Ok(values);
    }

    [HttpGet("GetById/{id:int}")]
    public IActionResult GetById(int id)
    {
        //if(!Values.ContainsKey(id))
        //    return NotFound();
        //return Ok(Values[id]);

        if (Values.TryGetValue(id, out var value))
            return Ok(value);
        return NotFound(new { id });
    }

    [HttpGet("count")]
    public int Count() => Values.Count;

    [HttpPost("add")]
    public IActionResult Add([FromBody] string value)
    {
        var id = Values.Count > 0
            ? Values.Max(v => v.Key) + 1
            : 1;
        Values.Add(id, value);
        var rout = nameof(GetById);
        _logger.LogInformation("Добавлено значение {0} c Id:{1}", value, id);
        return CreatedAtAction(rout, new { id }, value);
    }

    [HttpPut("Edit/{id:int}")]
    public IActionResult Edit(int id, [FromBody] string value)
    {
        if (!Values.ContainsKey(id))
        {
            _logger.LogWarning("Попытка редактирования отсутствующего значения с Id:{0}",id);
            return NotFound(new { id });
        }

        Values[id] = value;
        _logger.LogInformation("Выполнгено изменение значений \"{0}\" с Id:{1}", value, id);
        return Ok(new {value});
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        if (!Values.ContainsKey(id))
        {
            _logger.LogWarning("Попытка удалить не существующий элемент с Id:{0}", id);
            return NotFound(new { id });
        }
        var value = Values[id];
        Values.Remove(id);
        _logger.LogInformation("Удалёнл элемент с Id:{0}", id);
        return Ok(new { value });
    }
}