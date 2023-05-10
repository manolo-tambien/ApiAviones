using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiAviones.Modelos.DTO
{
	public class AeropuertoDTO
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del aeropuerto es requerido")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La cantidad de aforo es obligatoria.")]
        public string Aforo { get; set; }

        public DateTime FechaDeCreacion { get; set; }

        
        public int avionId { get; set; }
    }
}

