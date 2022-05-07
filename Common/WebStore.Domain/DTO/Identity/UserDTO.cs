using Microsoft.AspNetCore.Identity;
using WebStore.Domain.Entities.Identity;

namespace WebStore.Domain.DTO.Identity;

public class UserDto
{
    public User User { get; set; } = null!;
}

public class AddLoginDto : UserDto
{
    public UserLoginInfo UserLoginInfo { get; set; } = null!;
}

public class PasswordHashDto : UserDto
{
    public string Hash { get; set; } = null!;
}

public class SetLockoutDto : UserDto
{
    public DateTimeOffset? LockoutEnd { get; set; }
}