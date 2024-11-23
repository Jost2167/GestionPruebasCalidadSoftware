using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCalidadSoftware.Data;
using ProyectoCalidadSoftware.Models;
using System.Linq;

namespace ProyectoCalidadSoftware.Controllers
{
    public class PruebaController : Controller
    {
        private readonly AppDBContext _context;

        public PruebaController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CrearPrueba(int softwareId)
        {
            // Obtener el software a evaluar
            var software = _context.Software.FirstOrDefault(s => s.Id == softwareId);
            if (software == null)
            {
                return NotFound("Software no encontrado");
            }

            // Obtener criterios con sus preguntas
            var criterios = _context.Criterio
                .Include(c => c.Preguntas)
                .ToList();

            ViewBag.Software = software;
            ViewBag.Criterios = criterios;

            return View();
        }

        [HttpPost]
        public IActionResult CrearPrueba(int softwareId, Dictionary<string, int> respuestas, string nombrePrueba)
        {
            // Agrupar las respuestas por CriterioId
            var criteriosAgrupados = respuestas
                .GroupBy(r =>
                {
                    var partes = r.Key.Split('_');
                    return int.Parse(partes[0]); // CriterioId
                });

            foreach (var grupo in criteriosAgrupados)
            {
                var criterioId = grupo.Key;

                // Calcular el promedio de los puntajes
                var promedioPuntaje = grupo.Average(g => g.Value);

                // Crear y guardar una sola Prueba por Criterio
                var prueba = new Prueba
                {
                    CriterioId = criterioId,
                    SoftwareId = softwareId,
                    Nombre = nombrePrueba,  // Asignar el nombre de la prueba
                    Puntaje = (int)Math.Round(promedioPuntaje),
                    Fecha = DateTime.Now
                };

                _context.Prueba.Add(prueba);
            }

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            // Redirigir a DetalleSoftware con el id del software
            return RedirectToAction("DetalleSoftware", "Software", new { id = softwareId });
        }
    }
}
