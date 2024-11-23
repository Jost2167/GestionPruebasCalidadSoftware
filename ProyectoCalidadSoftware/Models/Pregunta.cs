using System.ComponentModel.DataAnnotations;

namespace ProyectoCalidadSoftware.Models
{
    public class Pregunta
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public int CriterioId { get; set; }
        public Criterio Criterio { get; set; }
    }

}
