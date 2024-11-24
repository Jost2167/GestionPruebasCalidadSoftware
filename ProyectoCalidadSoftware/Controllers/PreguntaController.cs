using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoCalidadSoftware.Data;
using ProyectoCalidadSoftware.Models;

namespace ProyectoCalidadSoftware.Controllers
{

    [Authorize(Roles = "Administrador")]
    public class PreguntaController : Controller
    {
        private readonly AppDBContext _appDBContext;
        public PreguntaController(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        // GET: Muestra las preguntas de un criterio específico
        public IActionResult VerPreguntas(int criterioId)
        {
            var criterio = _appDBContext.Criterio
                .Include(c => c.Preguntas)
                .FirstOrDefault(c => c.Id == criterioId);

            if (criterio == null)
            {
                return NotFound();
            }

            return View(criterio);
        }

        [HttpPost]
        public IActionResult CrearPregunta(Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                _appDBContext.Pregunta.Add(pregunta);

                var criterio = _appDBContext.Criterio.Find(pregunta.CriterioId);
                if (criterio != null)
                {
                    criterio.CantidadPreguntas++; // Incrementar el contador
                }

                _appDBContext.SaveChanges();

                return RedirectToAction("VerPreguntas", "Pregunta", new { criterioId = pregunta.CriterioId });
            }

            return View(pregunta);
        }

        [HttpPost]
        public IActionResult EditarPregunta(Pregunta pregunta)
        {
            if (ModelState.IsValid)
            {
                // Aquí debes actualizar la pregunta en la base de datos
                _appDBContext.Pregunta.Update(pregunta); // Si estás usando EF
                _appDBContext.SaveChanges();

                return RedirectToAction("VerPreguntas", "Pregunta", new { criterioId = pregunta.CriterioId });
            }
            return View(pregunta); // O redirige a una vista que maneje el error
        }

        [HttpPost]
        public IActionResult EliminarPregunta(int id)
        {
            // Busca la pregunta por su ID
            var pregunta = _appDBContext.Pregunta.Find(id);
            if (pregunta == null)
            {
                return NotFound();
            }

            // Reduce el contador de preguntas en el criterio
            var criterio = _appDBContext.Criterio.Find(pregunta.CriterioId);
            if (criterio != null)
            {
                criterio.CantidadPreguntas--;
            }

            // Elimina la pregunta y guarda los cambios
            _appDBContext.Pregunta.Remove(pregunta);
            _appDBContext.SaveChanges();

            // Redirige al listado de preguntas del criterio
            return RedirectToAction("VerPreguntas", new { criterioId = criterio.Id });
        }
    }
}