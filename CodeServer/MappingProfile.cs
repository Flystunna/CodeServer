using AutoMapper;
using CodeServer.Core.Models;
using CodeServer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeServer
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to Resource
            CreateMap<project, ProjectDTO>();
            CreateMap<sdlc_system, sdlcSystem>();

            // Resource to Domain
            CreateMap<ProjectDTO, project>();
            CreateMap<sdlcSystem, sdlc_system>();
        }
    }
}
