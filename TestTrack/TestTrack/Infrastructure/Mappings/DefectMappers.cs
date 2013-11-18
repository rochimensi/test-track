using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class DefectMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<Defect, DefectVM>();
            Mapper.CreateMap<DefectVM, Defect>();
            Mapper.CreateMap<ResultVM, Defect>();
        }
    }
}