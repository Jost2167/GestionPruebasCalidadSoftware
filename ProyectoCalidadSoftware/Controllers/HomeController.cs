using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization; 

namespace ProyectoCalidadSoftware.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Inicio()
        {
            return View();
        }

        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Iniciar", "Acceso");
        }
    }
}
