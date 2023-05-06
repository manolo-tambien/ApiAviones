using ApiAviones.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiAviones.Data
{
    public class ApplicationDBContext: DbContext
    {
        public  ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {

        }

        // Conforme se van creando tablas se agregan aquí las clases de los modelos.
        public DbSet<Avion> Avion { get; set; }

        public DbSet<Aeropuerto> Aeropuerto { get; set; }
    }
}
