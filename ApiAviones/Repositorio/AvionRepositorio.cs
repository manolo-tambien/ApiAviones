using ApiAviones.Data;
using ApiAviones.Modelos;
using ApiAviones.Repositorio.IRepositorio;

namespace ApiAviones.Repositorio

{
    public class AvionRepositorio : IAvionRepositorio
    {
        // Instancia de ApplicationDBContext de la base de datos.
        private readonly ApplicationDBContext _bd;

        /// <summary>
        /// Se accede a la base de datos a través de la variable que recibe el constructor, en este caso "_bd"
        /// </summary>
        public AvionRepositorio(ApplicationDBContext bd)
        {
            _bd = bd;
        }
        /// <summary>
        /// Actualiza la fecha de creacion del avion
        /// </summary>
        /// <param name="avion">Avion al que se le quiere actualizar la fecha de creacion</param>
        /// <returns>Verdadero cuando se actualiza la fecha de creacion. Falso cuando no.</returns>
        public bool ActualizarAvion(Avion avion)
        {
            avion.FechaCreacion = DateTime.Now;
            _bd.Avion.Update(avion);
            return Guardar();
        }
        /// <summary>
        /// Borra un avion de la tabla Aviones de la base de datos
        /// </summary>
        /// <param name="avion">El avion que se quiere elimniar.</param>
        /// <returns>Retorna verdadero cuando se borra un avion. Falso cuando no.</returns>
        public bool BorrarAvion(Avion avion)
        {
            _bd.Avion.Remove(avion);
            return Guardar();
        }
        /// <summary>
        /// Crea un avion en la base de datos
        /// </summary>
        /// <param name="avion">Avion que se quiere guardar</param>
        /// <returns>Devuleve verdadero cuando se guardó exitosamente un avion o falso cuando no se pudo guardar.</returns>
        public bool CrearAvion(Avion avion)
        {
            avion.FechaCreacion = DateTime.Now;
            _bd.Avion.Add(avion);
            return Guardar();
        }
        /// <summary>
        /// Si no encuentra el nombre del avión devuelve un boleano y viceversa.
        /// </summary>
        /// <param name="nombre">String del nombre que se quiere buscar en la tabla de aviones.</param>
        /// <returns>Devuelve bool si existe o no.</returns>
        public bool ExisteAvion(string nombre)
        {
            return _bd.Avion.Any(a => a.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            
        }   
        /// <summary>
        /// Busca en la tabla Avion los aviones que coinciden con el parametros id enviado
        /// </summary>
        /// <param name="id">Id del parámetro que se quiere buscar.</param>
        /// <returns>Devulve un valor booleano para indicar si existe el avion siendo buscado por id.</returns>
        public bool ExisteAvion(int id)
        {
            return _bd.Avion.Any(a => a.Id == id);
        }
        /// <summary>
        /// Obtiene el avion con el id del avion enviado
        /// </summary>
        /// <param name="avionId">Id del avión enviado</param>
        /// <returns>Retorna el avion buscado.</returns>
        public Avion GetAvion(int avionId)
        {
            return _bd.Avion.FirstOrDefault(avion => avion.Id == avionId);
        }
        /// <summary>
        /// Obtinene el listado de todos los aviones ordenados por el nombre
        /// </summary>
        /// <returns>Regresa la lista de todos los aviones filtrados por el nombre</returns>
        public ICollection<Avion> GetAviones()
        {
            return _bd.Avion.OrderBy(avion  => avion.Nombre).ToList();
        }
        /// <summary>
        /// El método de guardar se ejecuta para aplicar los cambios hechos en la base de datos
        /// y se valida que si hubo cambios, retorne verdadero y si no que retorne falso.
        /// </summary>
        /// <returns>Retorna verdadero para cuando detectó cambios en la base de datos y falso para cuando no.</returns>
        public bool Guardar() 
        {
            return _bd.SaveChanges() >= 0 ? true: false;
        }

    }
}
