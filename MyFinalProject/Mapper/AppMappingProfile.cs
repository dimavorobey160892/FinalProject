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
            CreateMap<Car, CarModel>();
        }
    }
}
