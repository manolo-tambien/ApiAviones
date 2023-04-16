using System.ComponentModel.DataAnnotations;

namespace ApiAviones.Modelos
{
    public class Avion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
