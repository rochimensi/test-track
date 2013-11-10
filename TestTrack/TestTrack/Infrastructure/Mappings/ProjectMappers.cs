using AutoMapper;
using Hiperion.Infrastructure.Automapper;
using TestTrack.Models;
using TestTrack.ViewModels;

namespace TestTrack.Infrastructure.Mappings
{
    public class ProjectMappers : IObjectMapperConfigurator
    {
        public void Apply()
        {
            Mapper.CreateMap<Project, ProjectVM>();

            Mapper.CreateMap<ProjectVM, Project>();
        }
    }
}