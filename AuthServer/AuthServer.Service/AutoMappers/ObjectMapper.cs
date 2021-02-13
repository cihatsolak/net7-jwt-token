using AutoMapper;
using System;

namespace AuthServer.Service.AutoMappers
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> _mapper = new Lazy<IMapper>(() =>
        {
            var mapperConfiguration = new MapperConfiguration(configuration =>
            {
                configuration.AddProfile<DtoMapper>();
            });

            return mapperConfiguration.CreateMapper();
        });

        public static IMapper Mapper => _mapper.Value;
    }
}
