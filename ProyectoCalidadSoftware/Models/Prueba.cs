namespace ProyectoCalidadSoftware.Models
{
    public class Prueba
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Puntaje { get; set; }

        public string ContenidoImpresion { get; set; }

        public DateTime Fecha { get; set; }

        // Referencia al software que se evalúa
        public int SoftwareId { get; set; }
        public Software Software { get; set; }

        // Referencia al criterio de evaluación
        public int CriterioId { get; set; }
        public Criterio Criterio { get; set; }
    }
}


