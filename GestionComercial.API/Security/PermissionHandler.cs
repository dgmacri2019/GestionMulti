using GestionComercial.Applications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GestionComercial.API.Security
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionService _permisoService;

        public PermissionHandler(IPermissionService permisoService)
        {
            _permisoService = permisoService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return;

            var tienePermiso = await _permisoService.UserHasPermissionAsync(userId, requirement.Permission);

            if (tienePermiso)
            {
                context.Succeed(requirement);
            }
        }
    }
}