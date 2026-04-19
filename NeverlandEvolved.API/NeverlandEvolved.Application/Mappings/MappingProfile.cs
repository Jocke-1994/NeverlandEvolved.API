using AutoMapper;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Domain.Entities;

namespace NeverlandEvolved.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Vi mappar från Game (Databas) -> GameDto (API)
            CreateMap<Game, GameDto>();
        }
    }
}