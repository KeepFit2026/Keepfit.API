using KeepFit.Backend.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace KeepFit.Backend.API.Filter;

public class AuthorizeRoleAttribute : AuthorizeAttribute
{
    public AuthorizeRoleAttribute(params UserRoles[] roles)
    {
        // On convertit les enums en int, puis en string, séparés par des virgules
        // Exemple : si on passe UserRole.Admin, cela devient "2"
        Roles = string.Join(",", roles.Select(r => ((int)r).ToString()));
    }
}