using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class ResultMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<Result, ResultVM>();

            Mapper.CreateMap<ResultVM, Result>();
        }
    }
}