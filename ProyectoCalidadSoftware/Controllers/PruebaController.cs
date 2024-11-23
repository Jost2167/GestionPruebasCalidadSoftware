using DinkToPdf;
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
            var criteriosAgrupados = respuestas
                .GroupBy(r =>
                {
                    var partes = r.Key.Split('_');
                    return int.Parse(partes[0]);
                });

            List<Prueba> pruebas = new List<Prueba>();

            foreach (var grupo in criteriosAgrupados)
            {
                var criterioId = grupo.Key;
                var promedioPuntaje = grupo.Average(g => g.Value);

                var prueba = new Prueba
                {
                    CriterioId = criterioId,
                    SoftwareId = softwareId,
                    Nombre = nombrePrueba,
                    Puntaje = (int)Math.Round(promedioPuntaje),
                    Fecha = DateTime.Now
                };

                pruebas.Add(prueba);
            }

            _context.Prueba.AddRange(pruebas);
            _context.SaveChanges();

            // Generar el contenido HTML para impresión del PDF
            var criterios = _context.Criterio.Include(c => c.Preguntas).ToList(); // Obtener criterios con preguntas
            string contenidoImpresion = GenerarContenidoImpresion(nombrePrueba, respuestas, criterios);

            // Definir la ruta del archivo PDF
            string carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "pdfs");
            if (!Directory.Exists(carpetaDestino))
            {
                Directory.CreateDirectory(carpetaDestino);
            }

            string nombreArchivo = $"{Guid.NewGuid()}.pdf";
            string rutaArchivo = Path.Combine(carpetaDestino, nombreArchivo);

            // Crear el PDF
            var converter = new BasicConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    Out = rutaArchivo,
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait
                },
                Objects = {
                    new ObjectSettings
                    {
                        HtmlContent = contenidoImpresion,
                        WebSettings = { DefaultEncoding = "utf-8" }
                    }
                }
            };

            // Convertir HTML a PDF
            converter.Convert(doc);

            // Guardar la URL del PDF generado en la base de datos
            var pruebaGenerada = pruebas.First();
            pruebaGenerada.ContenidoImpresion = nombreArchivo;
            _context.SaveChanges();

            return RedirectToAction("DetalleSoftware", "Software", new { id = softwareId });
        }

        // Método para generar el contenido HTML del PDF
        private string GenerarContenidoImpresion(
            string nombrePrueba,
            Dictionary<string, int> respuestas,
            List<Criterio> criterios)
        {
            var contenido = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    h1, h2, h3 {{ color: #333; }}
                    table {{ border-collapse: collapse; width: 100%; margin-top: 20px; }}
                    th, td {{ border: 1px solid #ddd; padding: 8px; text-align: left; }}
                    th {{ background-color: #f2f2f2; }}
                    ul {{ list-style-type: none; padding: 0; }}
                    li {{ margin-bottom: 5px; }}
                </style>
            </head>
            <body>
                <h1>Prueba: {nombrePrueba}</h1>
                <h2>Resultados</h2>
            ";

            // Recorrer los criterios y sus preguntas
            foreach (var criterio in criterios)
            {
                contenido += $@"
                <h3>Criterio: {criterio.Nombre}</h3>
                <p>{criterio.Descripcion}</p>
                <table>
                    <thead>
                        <tr>
                            <th>Pregunta</th>
                            <th>Respuesta</th>
                        </tr>
                    </thead>
                    <tbody>
                ";

                // Recorremos las preguntas del criterio
                foreach (var pregunta in criterio.Preguntas)
                {
                    var key = $"{criterio.Id}_{pregunta.Id}";
                    var respuesta = respuestas.ContainsKey(key) ? respuestas[key] : 0;

                    contenido += $@"
                    <tr>
                        <td>{pregunta.Texto}</td>
                        <td>{respuesta}</td>
                    </tr>
                    ";
                }

                contenido += "</tbody></table>";
            }

            contenido += @"
            </body>
            </html>
            ";

            return contenido;
        }
    }
}
