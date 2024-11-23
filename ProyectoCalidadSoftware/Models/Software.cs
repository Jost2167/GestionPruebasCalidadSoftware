namespace ProyectoCalidadSoftware.Models
{
    public class Software
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public string Descripcion { get; set; }
        public string ObjetivoGeneral { get; set; }
        public string ObjetivosEspecificos { get; set; }
        public string Version { get; set; }

        // Relación de uno a muchos con Evaluación
        public List<Prueba> Pruebas { get; set; } = new List<Prueba>();

    }
}


