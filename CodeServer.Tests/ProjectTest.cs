using AutoMapper;
using CodeServer.Business.Services;
using CodeServer.Controllers;
using CodeServer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using CodeServer.Core.Models;
using System.Collections.Generic;
using CodeServer.Data.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CodeServer.Data.Repositories;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using FakeItEasy;
using Moq;
using System.Threading.Tasks;

namespace CodeServer.Tests
{
    public class ProjectTest
    {
        private readonly ProjectsController _contr;
        private Mock<IProjectRepo> mockRepo;
        private Mock<IProjectService> mockSvc;

        private Mock<ISdlcSystemService> mocSdlckSvc;
        private Mock<ISdlc_SystemRepo> mockSdlcRepo;
        private Mock<ApplicationDbContext> mockDBRepo;
        private Mock<IUnitOfWork> mockUowRepo;
        private IMapper _mapper;

        private IList<project> projLisr;
        private IList<sdlc_system> sdlcList;
        public ProjectTest()
        {
            var mapp = new MappingProfile();
            var config = new MapperConfiguration(c => c.AddProfile(mapp));
            _mapper = new Mapper(config);

            mockRepo = new Mock<IProjectRepo>();
            mockSvc = new Mock<IProjectService>();
            mocSdlckSvc = new Mock<ISdlcSystemService>();

            mockSdlcRepo = new Mock<ISdlc_SystemRepo>();
            mockUowRepo = new Mock<IUnitOfWork>();

            mockDBRepo = new Mock<ApplicationDbContext>();
            sdlcList = new List<sdlc_system>()
            {
                new sdlc_system
                {
                        id = 1,
                        base_url = "hhhh",
                        description = "describe",
                        created_date = DateTime.Now
                }
            };

            projLisr = new List<project>()
            {
                new project
                {
                    id= 1,
                    external_id ="hhh",
                    name="test",
                    created_date = DateTime.Now,
                    last_modified_date = DateTime.Now,
                    sdlc_systemid = 1,
                    sdlc_system = new sdlc_system
                    {
                        id = 1,
                        base_url = "hhhh",
                        description = "describe",
                        created_date = DateTime.Now
                    },
                },
                new project
                {
                    id = 2,
                    external_id ="hhh2",
                    name="test2",
                    created_date = DateTime.Now,
                    last_modified_date = DateTime.Now,
                    sdlc_systemid = 2,
                    sdlc_system = new sdlc_system
                    {
                        id = 2,
                        base_url = "hhhh2",
                        description = "describe2",
                        created_date = DateTime.Now
                    },
                }
            };

            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(projLisr.ToList());

            mockRepo.Setup(repo => repo.GetProjectByIdAsync(It.IsAny<int>())).ReturnsAsync((int i)
                => projLisr.SingleOrDefault(x => x.id == i));

            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<project>())).Callback((project employeeDetails) =>
            {
                employeeDetails.id = projLisr.Count() + 1;
                projLisr.Add(employeeDetails);
            });

            mockRepo.SetupAllProperties();



            mockSvc.Setup(repo => repo.GetAllProjects()).ReturnsAsync(projLisr.ToList());

            mockSvc.Setup(repo => repo.GetProjectById(It.IsAny<int>())).ReturnsAsync((int i)
                          => projLisr.SingleOrDefault(x => x.id == i));

            mockSvc.Setup(repo => repo.CreateProject(It.IsAny<project>())).Callback((project newProj) =>
            {
                newProj.id = projLisr.Count() + 1;
                projLisr.Add(newProj);
            });

            mockSvc.SetupAllProperties();



            mocSdlckSvc.Setup(repo => repo.GetAllSdlcSystem()).ReturnsAsync(It.IsAny<IEnumerable<sdlc_system>>());

            mocSdlckSvc.Setup(repo => repo.GetSdlcSystemById(It.IsAny<int>())).ReturnsAsync((int i)
                          => sdlcList.SingleOrDefault(x => x.id == i));

            mocSdlckSvc.Setup(repo => repo.CreateSdlcSystem(It.IsAny<sdlc_system>())).Callback((sdlc_system sdlcSyst) =>
            {
                sdlcSyst.id = projLisr.Count() + 1;
                sdlcList.Add(sdlcSyst);
            });

            mocSdlckSvc.SetupAllProperties();


            _contr = new ProjectsController(mockSvc.Object, _mapper, mocSdlckSvc.Object);
        }
        private project CreateDefault()
        {
            return new project
            {
                id = 3,
                external_id = "hhh3",
                name = "test3",
                created_date = DateTime.Now,
                last_modified_date = DateTime.Now,
                sdlc_systemid = 3,
                sdlc_system = new sdlc_system
                {
                    id = 3,
                    base_url = "hhhh3",
                    description = "describe3",
                    created_date = DateTime.Now
                },
            };
        }
        private ProjectDTO CreateDefaultDTO()
        {
            return new ProjectDTO
            {
                id = 3,
                external_id = "hhh3",
                name = "test3",
                created_date = DateTime.Now,
                last_modified_date = DateTime.Now,
                sdlcSystem = new sdlcSystem
                {
                    id = 3,
                    base_url = "hhhh3",
                    description = "describe3",
                    created_date = DateTime.Now
                },
            };
        }
        [Fact]
        public async Task GetAllProject()
        {
            // Arrange
            // Act
            var employeeDetail = await _contr.GetAllProjects();

            Assert.IsType<OkObjectResult>(employeeDetail);
        }
        [Fact]
        public async Task GetProjectById()
        {
            //Arrange
            const int id = 1;

            // Act
            var result = await _contr.GetProject(1);

            var empList = result as OkObjectResult;

            var model = empList.Value as ProjectDTO;

            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(empList);
            Assert.Equal(model.id, id);
        }
        [Fact]
        public async Task CreateProject()
        {
            //Arrange

            var newMap = _mapper.Map(CreateDefault(), CreateDefaultDTO());
            // Act
            var result = await _contr.CreateProject(newMap);

            var empList = result as CreatedResult;

            Assert.IsType<CreatedResult>(result);

            Assert.NotNull(empList);
        }
    }
}
