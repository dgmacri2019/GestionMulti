using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using GestionComercial.API.Mappers;

namespace GestionComercial.API.Security
{
    public class AuthorizePermissionAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var controller = actionDescriptor.ControllerName;
            var action = actionDescriptor.ActionName;

            // 🧠 Mapear nombres técnicos a los permisos de la base de datos
            var permiso = $"{PermissionNameMapper.MapController(controller)}-{PermissionNameMapper.MapAction(action)}";

            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var user = context.HttpContext.User;

            var result = await authorizationService.AuthorizeAsync(user, null, new PermissionRequirement(permiso));

            if (!result.Succeeded)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}