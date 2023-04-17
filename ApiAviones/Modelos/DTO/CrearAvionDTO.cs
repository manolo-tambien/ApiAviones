using System.ComponentModel.DataAnnotations;

namespace ApiAviones.Modelos.DTO
{
    public class CrearAvionDTO
    {
        /// <summary>
        ///  Esta validación es importante para el nombre porque sino se crea vacia el nombre de categoria.
        /// </summary>
        [Required(ErrorMessage ="El nombre es obligatorio.")]
        [MaxLength(60,ErrorMessage ="El número máximo de caracteres es de 60")]
        public string Nombre { get; set; }
    }
}
