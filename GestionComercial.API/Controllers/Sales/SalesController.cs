﻿using GestionComercial.Infrastructure.Extensions;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionComercial.API.Controllers.Sales
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Developer, Administrator,Cajero")]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public SalesController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // POST: api/sales/generatesaleasync
        [HttpPost("GenerateSaleAsync")]
        public async Task<IActionResult> GenerateSaleAsync()
        {
            // Aquí colocarías la lógica para generar una venta.
            // Por ejemplo: validar datos, crear la venta en la base de datos, etc.

            // Simulación de respuesta
            return Ok(new { Message = "Venta generada correctamente" });
        }



        // POST: api/sales/cancelsaleasync
        [HttpPost("CancelSaleAsync")]
        public async Task<IActionResult> CancelSaleAsync()
        {
            // Obtener el usuario autenticado.
            IdentityUser? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Si el usuario pertenece al rol "Cajero", debe contar con el permiso especial "anular_ventas".
            if (await _userManager.IsInRoleAsync(user, "Cajero"))
                if (!IdentityUserExtensions.HasPermission(user, "anular_ventas", _context))
                    return Forbid();



            // Para los roles Developer y Administrador se asume que tienen acceso total.

            // Aquí colocarías la lógica para anular una venta.
            // Por ejemplo: buscar la venta, validarla, marcarla como anulada, etc.

            // Simulación de respuesta
            return Ok(new { Message = "Venta anulada correctamente" });
        }
    }
}
