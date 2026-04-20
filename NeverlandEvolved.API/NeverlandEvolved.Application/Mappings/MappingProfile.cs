using AutoMapper;
using NeverlandEvolved.Application.DTOs;
using NeverlandEvolved.Domain.Entities;

namespace NeverlandEvolved.Application.Mappings
{
    // MappingProfile definierar hur AutoMapper ska konvertera mellan entiteter och DTOs.
    // När egenskapsnamnen matchar exakt (t.ex. Game.Title -> GameDto.Title) behöver
    // vi inte konfigurera något extra — AutoMapper sköter det automatiskt.
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Game (entitet från databasen) -> GameDto (objekt som skickas till klienten)
            CreateMap<Game, GameDto>();

            // User -> UserDto (lösenord och känslig data ingår INTE i UserDto)
            CreateMap<User, UserDto>();

            // Review -> ReviewDto
            CreateMap<Review, ReviewDto>();
        }
    }
}