using AutoMapper;
using AutoStoreLib.Models;
using MyFinalProject.Models;
using System;

namespace MyFinalProject.Mapper
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<Car, CarModel>()
                .ForMember(x => x.Images, opt => opt.Ignore());

            CreateMap<CarModel, Car>()
                .ForMember(x => x.Images, opt => opt.Ignore())
                .ForMember(x => x.Questions, opt => opt.Ignore());
        }
    }
}
