using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

public class RequiereRolesAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public RequiereRolesAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // Verifica si el usuario tiene al menos uno de los roles requeridos
        var userRoles = context.HttpContext.User.Claims
            .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        if (!_roles.Any(r => userRoles.Contains(r)))
        {
            // El usuario no tiene los roles requeridos
            context.Result = new ForbidResult();
        }
    }
}

