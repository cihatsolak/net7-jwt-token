using AuthServer.Core.Domain;
using AuthServer.Core.DTOs;
using AutoMapper;

namespace AuthServer.Service.AutoMappers
{
    internal class DtoMapper : Profile
    {
        public DtoMapper()
        {
            ProductMap();
            UserMap();
        }

        private void ProductMap()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
        }

        private void UserMap()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
