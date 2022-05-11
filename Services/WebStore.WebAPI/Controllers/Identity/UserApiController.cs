using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Identity;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces;

namespace WebStore.WebAPI.Controllers.Identity;

[ApiController]
[Route(WebApiAddresses.V1.Identity.Users)]
public class UserApiController : ControllerBase
{
    private readonly UserStore<User, Role, WebStoreDb> _userStore;
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

    #region User

    [HttpPost("UserId")] // POST -> api/v1/users/UserId
    public async Task<string> GetUserIdAsync([FromBody] User user)
    {
        var userId = await _userStore.GetUserIdAsync(user);
        return userId;
    }

    [HttpPost("UserName")]
    public async Task<string> GetUserNameAsync([FromBody] User user)
    {
        var userName = await _userStore.GetUserNameAsync(user);
        return userName;
    }

    [HttpPost("UserName/{name}")]
    public async Task<string> SetUserNameAsync([FromBody] User user, string name)
    {
        await _userStore.SetUserNameAsync(user, name);
        await _userStore.UpdateAsync(user);
        return user.UserName;
    }

    [HttpPost("NormalizedUserName")]
    public async Task<string> GetNormalizedUserNameAsync([FromBody] User user)
    {
        var normalizedUserName = await _userStore.GetNormalizedUserNameAsync(user);
        return normalizedUserName;
    }

    [HttpPost("NormalizedUserName/{name}")]
    public async Task<string> SetNormalizedUserNameAsync([FromBody] User user, string name)
    {
        await _userStore.SetNormalizedUserNameAsync(user, name);
        await _userStore.UpdateAsync(user);
        return user.NormalizedUserName;
    }

    [HttpPost("User")]
    public async Task<bool> CreateAsync([FromBody] User user)
    {
        var creationResult = await _userStore.CreateAsync(user);

        if (!creationResult.Succeeded)
        {
            _logger.LogWarning("Ошибка добавления пользователя {0}:{1}", 
                user.UserName,
                string.Join(", ", creationResult.Errors.Select(e => e.Description)));
        }

        return creationResult.Succeeded;
    }

    [HttpPost("User/Delete")]       // POST   -> api/v1/identity/users/user/delete
    [HttpDelete("User/Delete")]     // DELETE -> api/v1/identity/users/user/delete
    [HttpDelete]                            // Delete -> api/v1/identity/users
    public async Task<bool> DeleteAsync([FromBody] User user)
    {
        var deleteResult = await _userStore.DeleteAsync(user);
        if(!deleteResult.Succeeded)
            _logger.LogWarning("Ошибка удаления пользователя {0}{1}", 
                user.UserName, 
                string.Join(", ", deleteResult.Errors.Select(e => e.Description)));
        return deleteResult.Succeeded;
    }

    [HttpGet("User/Find/{id}")]     // GET    -> api/v1/identity/users/user/find/CC4F8377-CE8A-4E5A-8604-BF5271F5F649
    public async Task<User> FindByIdAsync(string id)
    {
        var user = await _userStore.FindByIdAsync(id);
        return user;
    }

    [HttpGet("User/Normalize/{name}")] // GET    -> api/v1/identity/users/user/normalize/NAME
    public async Task<User> FindByNameAsync(string name)
    {
        var user = await _userStore.FindByNameAsync(name);
        return user;
    }

    public async Task AddToRoleAsync([FromBody] User user, string role /*, [FromServices] WebStoreDb db*/)
    {
        await _userStore.AddToRoleAsync(user, role);
        await _userStore.Context.SaveChangesAsync();
        // await db.SaveChangesAsync();
        // В старых версиях до .net5,6 нужно было
        // пробрасывать контекст БД через параметры для сохранения.
    }

    [HttpDelete("Role/{role}")]
    [HttpPost("Role/Delete{role}")]
    public async Task RemoveFromRoleAsync([FromBody] User user, string role)
    {
        await _userStore.DeleteAsync(user);
        await _userStore.Context.SaveChangesAsync();
    }

    [HttpPost("Roles")] // POST -> api/v1/identity/users/roles
    public async Task<IList<string>> GetRolesAsync([FromBody] User user)
    {
        var roles = await _userStore.GetRolesAsync(user);
        return roles;
    }

    [HttpGet("InRole/{role}")]
    public async Task<bool> IsInRoleAsync([FromBody] User user, string role)
    {
        var flag = await _userStore.IsInRoleAsync(user, role);
        return flag;
    }

    [HttpGet("UsersInRole/{role}")]
    public async Task<IList<User>> GetUsersInRoleAsync(string role)
    {
        var user = await _userStore.GetUsersInRoleAsync(role);
        return user;
    }

    [HttpPost("GetPasswordHash")]
    public async Task<string> GetPasswordHashAsync([FromBody] User user) => await _userStore.GetPasswordHashAsync(user);

    [HttpPost("SetPasswordHash")]
    public async Task<string> SetPasswordHashAsync([FromBody] PasswordHashDto hash)
    {
        await _userStore.SetPasswordHashAsync(hash.User, hash.Hash);
        await _userStore.UpdateAsync(hash.User);
        return hash.User.PasswordHash;
    }
    
    [HttpPost("HasPassword")]
    public async Task<bool> HasPasswordAsync([FromBody] User user) => await _userStore.HasPasswordAsync(user);
    #endregion

    #region Claims

    [HttpPost("GetClaims")]
    public async Task<IList<Claim>> GetClaimsAsync([FromBody] User user)
    {
        var claims = await _userStore.GetClaimsAsync(user);
        return claims;
    }

    [HttpPost("AddClaims")]
    public async Task AddClaimAsync([FromBody] ClaimDto claimInfo)
    {
        await _userStore.AddClaimsAsync(claimInfo.User, claimInfo.Claims);
        await _userStore.UpdateAsync(claimInfo.User);
    }

    [HttpPost("ReplaceClaim")]
    public async Task ReplaceClaimAsync([FromBody] ReplaceClaimDto claimInfo)
    {
        await _userStore.ReplaceClaimAsync(claimInfo.User, claimInfo.Claim, claimInfo.NewClaim);
        await _userStore.UpdateAsync(claimInfo.User);
    }

    [HttpPost("RemoveClaim")]
    public async Task RemoveClaim([FromBody] ClaimDto claimInfo)
    {
        await _userStore.RemoveClaimsAsync(claimInfo.User, claimInfo.Claims);
        await _userStore.UpdateAsync(claimInfo.User);
    }

    [HttpPost("GetUsersForClaim")]
    public async Task<IList<User>> GetUsersForClaimAsync([FromBody] Claim claim)
    {
        var users = await _userStore.GetUsersForClaimAsync(claim);
        return users;
    }

    #endregion

    #region TwoFactor

    [HttpPost("GetTwoFactorEnabled")]
    public async Task<bool> GetTwoFactorEnabledAsync([FromBody] User user)
    {
        var flag = await _userStore.GetTwoFactorEnabledAsync(user);
        return flag;
    }

    public async Task<bool> SetTwoFactorEnabledAsync([FromBody] User user, bool enabled)
    {
        await _userStore.SetTwoFactorEnabledAsync(user, enabled);
        await _userStore.UpdateAsync(user);
        return user.TwoFactorEnabled;
    }

    #endregion

    #region Email/Phone

    [HttpPost("GetEmail")]
    public async Task<string> GetEmailAsync([FromBody] User user)
    {
        var email = await _userStore.GetEmailAsync(user);
        return email;

    }
    [HttpPost("SetEmail/{email}")]
    public async Task<string> SetEmailAsync([FromBody] User user, string email)
    {
        await _userStore.SetEmailAsync(user, email);
        await _userStore.UpdateAsync(user);
        return user.Email;
    }

    [HttpPost("GetEmailConfirmed")]
    public async Task<bool> GetEmailConfirmedAsync([FromBody] User user)
    {
        var flag = await _userStore.GetEmailConfirmedAsync(user);
        return flag;
    }

    [HttpPost("SetEmailConfirmed/{enable}")]
    public async Task<bool> SetEmailConfirmedAsync([FromBody] User user, bool enable)
    {
        await _userStore.SetEmailConfirmedAsync(user, enable);
        await _userStore.UpdateAsync(user);
        return user.EmailConfirmed;
    }


    [HttpPost("GetNormalizedEmail")]
    public async Task<string> GetNormalizedEmailAsync(User user)
    {
        var normalizedEmail = await _userStore.GetNormalizedEmailAsync(user);
        return normalizedEmail;
    }

    [HttpPost("SetNormalizedEmail/{email?}")]
    public async Task<string> SetNormalizedEmailAsync([FromBody] User user, string? email)
    {
        await _userStore.SetNormalizedEmailAsync(user, email);
        await _userStore.UpdateAsync(user);
        return user.NormalizedEmail;
    }

    [HttpGet("UserFindByEmail/{email}")]
    public async Task<User> FindByEmailAsync(string email)
    {
        var user = await _userStore.FindByEmailAsync(email);
        return user;
    }

    [HttpPost("GetPhoneNumber")]
    public async Task<string> GetPhoneUmberAsync([FromBody] User user)
    {
        var phoneNumber = await _userStore.GetPhoneNumberAsync(user);
        return phoneNumber;
    }

    [HttpPost("SetPhoneNumber/{phone}")]
    public async Task<string> SetPhoneNumberAsync([FromBody] User user, string phone)
    {
        await _userStore.SetPhoneNumberAsync(user, phone);
        await _userStore.UpdateAsync(user);
        return user.PhoneNumber;
    }

    [HttpPost("GetPhoneNumberConfirmed")]
    public async Task<bool> GetPhoneNumberConfirmed([FromBody] User user)
    {
        var flag = await _userStore.GetPhoneNumberConfirmedAsync(user);
        return flag;
    }

    [HttpPost("SetPhoneNumberConfirmed/{confirmed}")]
    public async Task<bool> SetPhoneNumberConfirmedAsync([FromBody] User user, bool confirmed)
    {
        await _userStore.SetPhoneNumberConfirmedAsync(user, confirmed);
        await _userStore.UpdateAsync(user);
        return user.PhoneNumberConfirmed;
    }

    #endregion

    #region Login/Lockout

    [HttpPost("AddLogin")]
    public async Task AddLoginAsync([FromBody] AddLoginDto login /*, [FromServices]  WebStoreDb db*/)
    {
        await _userStore.AddLoginAsync(login.User, login.UserLoginInfo);
        await _userStore.Context.SaveChangesAsync();
        //await db.SaveChangeAsync();
    }

    [HttpPost("RemoveLogin/{LoginProvider}/{ProviderKey}")]
    public async Task RemoveLoginAsync([FromBody] User user, string loginProvider, string providerKey/*, [FromServices]  WebStoreDb db*/)
    {
        await _userStore.RemoveLoginAsync(user, loginProvider, providerKey);
        await _userStore.Context.SaveChangesAsync();
        //await db.SaveChangeAsync();
    }

    [HttpPost("GetLogins")]
    public async Task<IList<UserLoginInfo>> GetLoginsAsync([FromBody] User user, string loginProvider, string providerKey)
    {
        var logins = await _userStore.GetLoginsAsync(user);
        return logins;
    }

    [HttpGet("User/FindByLogin/{loginProvider}/{providerKey}")]
    public async Task<User> FindByLoginAsync(string loginProvider, string providerKey)
    {
        var user = await _userStore.FindByLoginAsync(loginProvider, providerKey);
        return user;
    }

    [HttpPost("GetLockoutEndDate")]
    public async Task<DateTimeOffset?> GetLockoutEndDateAsync([FromBody] User user)
    {
        var lockoutEndDate = await _userStore.GetLockoutEndDateAsync(user);
        return lockoutEndDate;
    }

    [HttpPost("SetLockoutEndDate")]
    public async Task<DateTimeOffset?> SetLockoutEndDateAsync([FromBody] SetLockoutDto lockoutInfo)
    {
        await _userStore.SetLockoutEndDateAsync(lockoutInfo.User, lockoutInfo.LockoutEnd);
        await _userStore.UpdateAsync(lockoutInfo.User);
        return lockoutInfo.User.LockoutEnd;
    }

    [HttpPost("IncrementAccessFailedCount")]
    public async Task<int> IncrementAccessFailedCountAsync([FromBody] User user)
    {
        var count = await _userStore.IncrementAccessFailedCountAsync(user);
        await _userStore.UpdateAsync(user);
        return count;
    }

    [HttpPost("ResetAccessFailedCount")]
    public async Task<int> ResetAccessFailedCountAsync([FromBody] User user)
    {
        await _userStore.ResetAccessFailedCountAsync(user);
        await _userStore.UpdateAsync(user);
        return user.AccessFailedCount;
    }
    
    [HttpPost("GetAccessFiledCount")]
    public async Task<int> GetAccessFiledCountAsync([FromBody] User user)
    {
        var count = await _userStore.GetAccessFailedCountAsync(user);
        return count;
    }

    [HttpPost("GetLockoutEnabled")]
    public async Task<bool> GetLockoutEnabledAsync([FromBody] User user)
    {
        var flag = await _userStore.GetLockoutEnabledAsync(user);
        return flag;
    }

    [HttpPost("SetLockoutEnabled/{enable}")]
    public async Task<bool> SetLockoutEnabledAsync([FromBody] User user, bool enable)
    {
        await _userStore.SetLockoutEnabledAsync(user, enable);
        await _userStore.UpdateAsync(user);
        return user.LockoutEnabled;
    }
    #endregion
}