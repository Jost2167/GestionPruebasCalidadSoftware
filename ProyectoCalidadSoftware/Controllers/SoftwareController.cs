using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCalidadSoftware.Data;
using ProyectoCalidadSoftware.Models;


namespace ProyectoCalidadSoftware.Controllers
{
    public class SoftwareController : Controller
    {
        private readonly AppDBContext _context;

        public SoftwareController(AppDBContext context)
        {
            _context = context;
        }

        //REALIZAR ESTO
        public IActionResult VerEmpresaSoftware()
        {
            // Incluye las empresas relacionadas con los softwares
            var empresasConSoftwares = _context.Empresa
                .Include(e => e.Softwares) // Incluye los softwares relacionados
                .ToList();

            return View(empresasConSoftwares);
        }


        public IActionResult DetalleSoftware(int id)
        {
            var software = _context.Software
                .Include(s => s.Empresa) // Incluye la empresa relacionada
                .Include(s => s.Pruebas)
                    .ThenInclude(p => p.Criterio) // Incluye los criterios de las pruebas
                .AsNoTracking()
                .FirstOrDefault(s => s.Id == id);

            if (software == null)
            {
                return NotFound("El software solicitado no existe.");
            }

            return View(software);
        }






        [HttpPost]
        public IActionResult CrearSoftware(Software software)
        {
            if (ModelState.IsValid)
            {
                software.Nombre = software.Nombre.ToUpper();
                _context.Software.Add(software);
                _context.SaveChanges();
                return RedirectToAction("VerSoftwareEmpresa", "Empresa", new { id = software.EmpresaId });
            }

            return View(software);
        }

        [HttpPost]
        public IActionResult EditarSoftware(Software software)
        {
            if (ModelState.IsValid)
            {
                software.Nombre = software.Nombre.ToUpper();

                _context.Software.Update(software);
                _context.SaveChanges();
                return RedirectToAction("VerSoftwareEmpresa", "Empresa", new { id = software.EmpresaId });
            }
            return View(software);
        }

        [HttpPost]
        public IActionResult EliminarSoftware(int id)
        {
            var software = _context.Software
                .Include(s => s.Pruebas)
                .FirstOrDefault(s => s.Id == id);

            if (software != null)
            {
                _context.Prueba.RemoveRange(software.Pruebas);
                _context.Software.Remove(software);
                _context.SaveChanges();
            }

            return RedirectToAction("VerSoftwareEmpresa", "Empresa", new { id = software.EmpresaId });
        }



    }
}
