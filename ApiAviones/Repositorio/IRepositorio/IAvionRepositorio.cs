using ApiAviones.Modelos;

namespace ApiAviones.Repositorio.IRepositorio
{
    public interface IAvionRepositorio
    {
        ICollection<Avion> GetAviones();
        Avion GetAvion(int avionId);
        bool ExisteAvion(string nombre);
        bool ExisteAvion(int id);
        bool CrearAvion(Avion avion);
        bool ActualizarAvion(Avion avion);
        bool BorrarAvion(Avion avion);
        bool Guardar();
    }
}
