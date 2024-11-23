namespace ProyectoCalidadSoftware.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }

        // Relación de uno a muchos con Software
        public List<Software> Softwares { get; set; } = new List<Software>();
    }
}
