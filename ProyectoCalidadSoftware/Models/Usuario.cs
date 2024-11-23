using Microsoft.Identity.Client;

namespace ProyectoCalidadSoftware.Models
{
    public class Usuario
    {
        public int Id { get;}
        public string Correo { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Rol {  get; set; }
        
    }
}
