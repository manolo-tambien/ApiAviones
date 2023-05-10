using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAviones.Modelos
{
    public class Aeropuerto
    {
        [Key]
        public int Id { get; set; }

        
        public string Nombre { get; set; }

        
        public string Aforo { get; set; }

        public DateTime FechaDeCreacion { get; set; }

        [ForeignKey("avionId")]
        public int avionId { get; set; }
        public Avion Avion { get; set; }
    }
}