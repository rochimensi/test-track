using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class TestPlanMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<TestPlan, TestPlanVM>();

            Mapper.CreateMap<TestPlanVM, TestPlan>();
        }
    }
}