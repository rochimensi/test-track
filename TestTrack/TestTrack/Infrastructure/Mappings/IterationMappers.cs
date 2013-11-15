using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class IterationMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<Iteration, IterationVM>();

            Mapper.CreateMap<IterationVM, Iteration>();
        }
    }
}