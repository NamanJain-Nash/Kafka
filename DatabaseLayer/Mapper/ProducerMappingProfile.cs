using AutoMapper;
using DatabaseLayer.Data;
using DatabaseLayer.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DatabaseLayer.Mapper
{
    public class ProducerMappingProfile : Profile
    {
        public ProducerMappingProfile()
        {
            //Mapping Profile Defined
            CreateMap<Producer, ProducerDTO>().ReverseMap();
        }
    }
}
