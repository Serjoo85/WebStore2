using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity;

[ApiController]
[Route(WebApiAddresses.V1.Identity.Users)]
public class UserApiController : ControllerBase
{
    private readonly UserStore<User,Role, WebStoreDb> _userStore;
    private readonly ILogger<UserApiController> _logger;

    public UserApiController(WebStoreDb db, ILogger<UserApiController> logger)
    {
        _userStore = new(db);
        _logger = logger;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userStore.Users.ToArrayAsync();
        return Ok(users);
    }
}