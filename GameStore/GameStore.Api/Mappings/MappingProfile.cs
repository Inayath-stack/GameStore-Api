using AutoMapper;
using GameStore.Api.Models;
using GameStore.Api.Dtos;

namespace GameStore.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Game,GameDto>();
        CreateMap<CreateGameDto,Game>();
        CreateMap<UpdateGameDto,Game>();
    }
}