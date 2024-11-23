using Microsoft.AspNetCore.Mvc;
using ProyectoCalidadSoftware.Data;
using ProyectoCalidadSoftware.Models;
using ProyectoCalidadSoftware.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ProyectoCalidadSoftware.Controllers
{
    public class AccesoController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public AccesoController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(UsuarioVM usuarioVM)
        {
            if(usuarioVM.Clave!=usuarioVM.ConfirmarClave)
            {
                ViewData["Mensaje"] = "Las contraseña no coinciden.";
                return View();
            }

            Usuario usuario = new Usuario()
            {
                Nombre = usuarioVM.Nombre,
                Correo= usuarioVM.Correo,
                Clave = usuarioVM.Clave,
                Rol= "Cliente"
            };

            await _appDBContext.Usuario.AddAsync(usuario);
            await _appDBContext.SaveChangesAsync();

            if (usuario.Id != 0) return RedirectToAction("Iniciar","Acceso");

            ViewData["Mensaje"] = "No se ha podido crear el usuario.";

            return View();
        }

        [HttpGet]
        public IActionResult Iniciar()
        {
            if (User.Identity!.IsAuthenticated) return RedirectToAction("Inicio", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Iniciar(UsuarioVM usuarioVM)
        {
            Usuario? usuario = await _appDBContext.Usuario
                                .Where(u =>
                                    u.Correo == usuarioVM.Correo &&
                                    u.Clave == usuarioVM.Clave
                                ).FirstOrDefaultAsync();

            if (usuario == null)
            {
                ViewData["Mensaje"] = "Correo o contraseña incorrecta.";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim("Correo", usuario.Correo),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                properties
                );

            return RedirectToAction("Inicio", "Home");
        }

        public IActionResult AccesoDenegado()
        {
            return View();
        }

    }
}
 