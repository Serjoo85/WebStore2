using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.DTO;

public class UserDTO
{
    public User User { get; set; } = null!;
}

public class AddLogginDTO : UserDTO
{
    public UserLoginInfo UserLoginInfo { get; set; } = null!;
}

public class PasswordHashDTO : UserDTO
{
    public string Hash { get; set; } = null!;
}

public class SetLockoutDTO : UserDTO
{
    public DateTimeOffset? LockoutEnd { get; set; }
}