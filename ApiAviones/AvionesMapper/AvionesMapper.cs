
using ApiAviones.Modelos;
using ApiAviones.Modelos.DTO;
using AutoMapper;

namespace ApiAviones.AvionesMapper
{
    public class AvionesMapper : Profile
    {
        public AvionesMapper()
        {
            CreateMap<Avion, AvionDTO>().ReverseMap();
            CreateMap<Avion, CrearAvionDTO>().ReverseMap();
        }
    }
}
