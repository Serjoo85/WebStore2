using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity;

[ApiController]
[Route(WebApiAddresses.V1.Identity.Roles)]
public class RolesApiController : ControllerBase
{
    private readonly ILogger<RolesApiController> _logger;
    private readonly RoleStore<Role> _roleStore;

    public RolesApiController(WebStoreDb db, ILogger<RolesApiController> logger)
    {
        _logger = logger;
        _roleStore = new(db);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllRole()
    {
        var roles = await _roleStore.Roles.ToArrayAsync();
        return Ok(roles);
    }
}