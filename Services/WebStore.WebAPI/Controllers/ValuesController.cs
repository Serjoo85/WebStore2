using System.ComponentModel;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
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
        public IEnumerable<string> GetAll() => Values.Values;

        [HttpGet("GetById/{id:int}")]
        public IActionResult GetById(int id)
        {
            //if(!Values.ContainsKey(id))
            //    return NotFound();
            //return Ok(Values[id]);
            if (Values.TryGetValue(id, out var value))
                return Ok(value);
            return NotFound();
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
            return CreatedAtAction(rout, new { id }, value);
        }

        [HttpPut("Edit/{id:int}")]
        public IActionResult Edit(int id, [FromBody] string value)
        {
            if (!Values.ContainsKey(id))
                return NotFound();
            Values[id] = value;
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            if (!Values.ContainsKey(id))
                return NotFound();
            Values.Remove(id);
            return Ok();
        }
    }
}
