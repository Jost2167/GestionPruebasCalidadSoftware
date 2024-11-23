namespace ProyectoCalidadSoftware.Models
{
    public class Criterio
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int CantidadPreguntas { get; set; }
        public List<Pregunta> Preguntas { get; set; } = new List<Pregunta>();
    }

}
