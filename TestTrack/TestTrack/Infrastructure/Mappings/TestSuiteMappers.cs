using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class TestSuiteMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<TestSuite, TestSuiteVM>();

            Mapper.CreateMap<TestSuiteVM, TestSuite>();
        }
    }
}