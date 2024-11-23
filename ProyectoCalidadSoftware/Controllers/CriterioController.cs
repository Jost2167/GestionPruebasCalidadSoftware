using Microsoft.AspNetCore.Mvc;
using ProyectoCalidadSoftware.Models;
using ProyectoCalidadSoftware.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoCalidadSoftware.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class CriterioController : Controller
    {
        private readonly AppDBContext _appDBContext;

        public CriterioController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public IActionResult VerCriterios()
        {
            // Obtén todos los criterios con sus preguntas
            var criterios = _appDBContext.Criterio
                                        .Include(c => c.Preguntas)  // Asegúrate de incluir las preguntas relacionadas
                                        .ToList();

            return View(criterios);
        }

        // GET: Muestra el formulario para crear criterios y preguntas
        public IActionResult CrearCriterio()
        {
            var model = new Criterio(); // Usamos el modelo Criterio directamente
            return View(model);
        }

        // POST: Guarda el criterio y las preguntas
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearCriterio(Criterio model)
        {
            if (ModelState.IsValid)
            {
                // Crear el nuevo criterio
                var criterio = new Criterio
                {
                    Nombre = model.Nombre.ToUpper(),  // Asignar nombre en mayúsculas
                    Descripcion = model.Descripcion,   // Asignar descripción
                    CantidadPreguntas = model.CantidadPreguntas
                };

                // Agregar preguntas al criterio
                foreach (var pregunta in model.Preguntas)
                {
                    var nuevaPregunta = new Pregunta
                    {
                        Texto = pregunta.Texto,  // Asignar el texto de la pregunta
                    };
                    criterio.Preguntas.Add(nuevaPregunta);  // Agregar la pregunta al criterio
                }

                // Guardar el nuevo criterio con sus preguntas en la base de datos
                _appDBContext.Criterio.Add(criterio);
                await _appDBContext.SaveChangesAsync();

                return RedirectToAction("VerCriterios", "Criterio");
            }

            return View(model);  // Si no es válido, vuelve a la vista con los datos
        }

        [HttpPost]
        public IActionResult EditarCriterio(int id, string nombre, string descripcion)
        {
            // Buscar el criterio en la base de datos por su Id
            var criterio = _appDBContext.Criterio.Find(id);
            if (criterio == null)
            {
                return NotFound();
            }

            // Actualizar los valores del criterio
            criterio.Nombre = nombre;
            criterio.Descripcion = descripcion;

            // Guardar los cambios en la base de datos
            _appDBContext.SaveChanges();

            // Redirigir a la página principal de criterios después de editar
            return RedirectToAction("VerCriterios", "Criterio");
        }

        // Eliminar un criterio y todas sus preguntas
        public async Task<IActionResult> EliminarCriterio(int id)
        {
            var criterio = await _appDBContext.Criterio
                                               .Include(c => c.Preguntas)
                                               .FirstOrDefaultAsync(c => c.Id == id);

            if (criterio != null)
            {
                _appDBContext.Criterio.Remove(criterio);
                await _appDBContext.SaveChangesAsync();
            }

            return RedirectToAction("VerCriterios");
        }
    }
}
