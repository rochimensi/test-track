using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class TeamMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<Team, TeamVM>();

            Mapper.CreateMap<TeamVM, Team>();
        }
    }
}