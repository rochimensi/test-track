using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class TestRunMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<TestRun, TestRunVM>();

            Mapper.CreateMap<TestRunVM, TestRun>();
        }
    }
}