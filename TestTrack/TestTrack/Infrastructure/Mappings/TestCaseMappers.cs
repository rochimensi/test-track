using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class TestCaseMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<TestCase, TestCaseVM>();

            Mapper.CreateMap<TestCaseVM, TestCase>();
        }
    }
}