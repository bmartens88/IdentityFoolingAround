using Microsoft.AspNetCore.Identity;

namespace IdentityFoolingAround.Data.Models;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
}