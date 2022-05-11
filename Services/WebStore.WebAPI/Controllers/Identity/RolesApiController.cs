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

    /*--------------------------------------------------------------------*/
    [HttpPost]
    public async Task<bool> CreateAsync(Role role)
    {
        var creationResult = await _roleStore.CreateAsync(role);
        if (!creationResult.Succeeded)
        {
            _logger.LogWarning("Ошибка добавления роли {0}:{1}",
                role.Name,
                string.Join(", ", creationResult.Errors.Select(e => e.Description)));
        }
        return creationResult.Succeeded;
    }

    [HttpPut]
    public async Task<bool> UpdateAsync(Role role)
    {
        var updateResult = await _roleStore.UpdateAsync(role);
        if (!updateResult.Succeeded)
        {
            _logger.LogWarning("Ошибка изменения ползователя {0}:{1}",
                role.Name,
                string.Join(", ", updateResult.Errors.Select(e => e.Description)));
        }
        return updateResult.Succeeded;
    }

    [HttpDelete]
    [HttpPost("Delete")]
    public async Task<bool> DeleteAsync(Role role)
    {
        var deleteResult = await _roleStore.DeleteAsync(role);
        if (!deleteResult.Succeeded)
        {
            _logger.LogWarning("Ошибка удаления роли {0}:{1}",
                role.Name,
                string.Join(", ", deleteResult.Errors.Select(e => e.Description)));
        }
        return deleteResult.Succeeded;
    }

    [HttpPost("GetRoleId")]
    public async Task<string> GetRoleIdAsync([FromBody] Role role)
    {
        var id = await _roleStore.GetRoleIdAsync(role);
        return id;
    }


    [HttpPost("GetRoleName")]
    public async Task<string> GetRoleNameAsync([FromBody] Role role)
    {
        var name = await _roleStore.GetRoleNameAsync(role);
        return name;
    }

    [HttpPost("SetRoleName/{name}")]
    public async Task<string> SetRoleNameAsync(Role role, string name)
    {
        await _roleStore.SetRoleNameAsync(role, name);
        await _roleStore.UpdateAsync(role);
        return role.Name;
    }

    [HttpPost("GetNormalizedRoleName")]
    public async Task<string> GetNormalizedRoleNameAsync(Role role)
    {
        var normalizedName = await _roleStore.GetNormalizedRoleNameAsync(role);
        return normalizedName;
    }

    [HttpPost("SetNormalizedRoleName/{name}")]
    public async Task<string> SetNormalizedRoleNameAsync(Role role, string name)
    {
        await _roleStore.SetNormalizedRoleNameAsync(role, name);
        await _roleStore.UpdateAsync(role);
        return role.NormalizedName;
    }

    [HttpGet("FindById/{id}")]
    public async Task<Role> FindByIdAsync(string id)
    {
        return await _roleStore.FindByIdAsync(id);
    }

    [HttpGet("FindByName/{name}")]
    public async Task<Role> FindByNameAsync(string name)
    {
        return await _roleStore.FindByNameAsync(name);
    }
}