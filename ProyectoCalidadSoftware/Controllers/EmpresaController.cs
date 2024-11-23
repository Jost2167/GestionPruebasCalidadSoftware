using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCalidadSoftware.Data;
using ProyectoCalidadSoftware.Models;
using System;

namespace ProyectoCalidadSoftware.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly AppDBContext _context;

        public EmpresaController(AppDBContext context)
        {
            _context = context;
        }

        // Listar todas las empresas
        public IActionResult VerEmpresas()
        {
            var empresas = _context.Empresa.Include(e => e.Softwares).ToList();
            return View(empresas);
        }

        // Ver detalles de una empresa y sus softwares
        public IActionResult VerSoftwareEmpresa(int id)
        {
            var empresa = _context.Empresa
                .Include(e => e.Softwares)
                .AsNoTracking() // Evita problemas de seguimiento
                .FirstOrDefault(e => e.Id == id);

            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        [HttpPost]
        public IActionResult CrearEmpresa(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                empresa.Nombre = empresa.Nombre.ToUpper();

                _context.Empresa.Add(empresa);
                _context.SaveChanges();
                return RedirectToAction("VerEmpresas","Empresa");
            }
            return View(empresa);
        }

        // POST: Guardar cambios al editar
        [HttpPost]
        public IActionResult EditarEmpresa(Empresa empresa)
        {
            if (empresa == null || empresa.Id == 0)
            {
                return BadRequest("La empresa enviada no es válida.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Empresa.Update(empresa);
                    _context.SaveChanges();
                    return RedirectToAction("VerEmpresas", "Empresa");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Manejo de errores de concurrencia
                    ModelState.AddModelError("", "Hubo un problema al actualizar los datos. Intente nuevamente.");
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Ocurrió un error inesperado.");
                    Console.WriteLine(ex.Message);
                }
            }

            // Retornar a la vista original si el modelo no es válido
            return View("VerEmpresas", _context.Empresa.Include(e => e.Softwares).ToList());
        }

        // POST: Eliminar empresa
        [HttpPost]
        public IActionResult EliminarEmpresa(int id)
        {
            var empresa = _context.Empresa
                .Include(e => e.Softwares)
                .ThenInclude(s => s.Pruebas)
                .FirstOrDefault(e => e.Id == id);

            if (empresa == null)
            {
                return NotFound();
            }

            _context.Empresa.Remove(empresa);
            _context.SaveChanges();
            return RedirectToAction("VerEmpresas");
        }
    }
}



