using System.Security.Claims;

namespace WebStore.Domain.DTO.Identity;

public class ClaimDto : UserDto
{
    public IEnumerable<Claim> Claims { get; set; } = null!;
}

public class ReplaceClaimDto : UserDto
{
    public Claim Claim { get; set; } = null!;

    public Claim NewClaim { get; set; } = null!;
}