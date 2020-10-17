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
using System.Collections;
using System.Data;
using Shouldly;

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
                },
                new sdlc_system
                {
                    id = 2,
                    base_url = "hhhh2",
                    description = "describe2",
                    created_date = DateTime.Now
                },
                new sdlc_system
                {
                    id = 3,
                    base_url = "hhhh3",
                    description = "describe3",
                    created_date = DateTime.Now
                },
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

            mockSvc.Setup(repo => repo.GetProjectById(It.IsAny<int>())).ReturnsAsync((int i) => projLisr.SingleOrDefault(x => x.id == i));

            mockSvc.Setup(repo => repo.GetProjectByIdAsNoTracking(It.IsAny<int>())).ReturnsAsync((int i) => projLisr.SingleOrDefault(x => x.id == i));

            mockSvc.Setup(repo => repo.CreateProject(It.IsAny<project>())).Callback((project newProj) =>
            {
                newProj.id = projLisr.Count() + 1;
                projLisr.Add(newProj);

            }).ReturnsAsync(CreateDefault());

            mockSvc.SetupAllProperties();



            mocSdlckSvc.Setup(repo => repo.GetAllSdlcSystem()).ReturnsAsync(It.IsAny<IEnumerable<sdlc_system>>());

            mocSdlckSvc.Setup(repo => repo.GetSdlcSystemById(It.IsAny<int>())).ReturnsAsync((int i) => sdlcList.SingleOrDefault(x => x.id == i));
           
            mocSdlckSvc.Setup(repo => repo.GetSdlcSystemByIdAsNoTracking(It.IsAny<int>())).ReturnsAsync((int i) => sdlcList.SingleOrDefault(x => x.id == i));

            mocSdlckSvc.Setup(repo => repo.CreateSdlcSystem(It.IsAny<sdlc_system>())).Callback((sdlc_system sdlcSyst) =>
            {
                sdlcSyst.id = projLisr.Count() + 1;
                sdlcList.Add(sdlcSyst);
            });

            mocSdlckSvc.SetupAllProperties();
           

            _contr = new ProjectsController(mockSvc.Object, _mapper, mocSdlckSvc.Object);
            _contr.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            _contr.ControllerContext.HttpContext = new DefaultHttpContext();
            _contr.ControllerContext.HttpContext.Request.Scheme = "http";
            _contr.ControllerContext.HttpContext.Request.Host = new HostString("localhost");
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

            mockSvc.Verify(s => s.GetAllProjects(), Moq.Times.Once());

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

            //Assert

            mockSvc.Verify(s => s.GetProjectById(id),  Moq.Times.Once());

            Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(empList);
            Assert.Equal(model.id, id);
        }
        [Fact]
        public async Task CreateProject()
        {
            //Arrange
            var newDTO = CreateDefaultDTO();
            var newproj = CreateDefault();

            var newMap = _mapper.Map(newproj, newDTO);

            // Act
            var result = await _contr.CreateProject(newMap);

            //Assert

            var empList = result as CreatedResult;

            Assert.IsType<CreatedResult>(result);

            Assert.NotNull(empList);
        }

        [Fact]
        public void TestNullDataTables()
        {
            DataTable data = new DataTable();

            data = null;
            //post null datatable
            Should.Throw<ArgumentNullException>(() =>
            {

                DataTableHelpers.GetDataTableColumns(data);

            });
        }
        [Fact]
        public void TestDataTables()
        {
            DataTable data = new DataTable();

            data.Columns.Add("NewColumn", typeof(System.Int32));

            foreach (DataRow row in data.Rows)
            {
                //need to set value to NewColumn column
                row["NewColumn"] = 0;   // or set it to some other value
            };

            var send = DataTableHelpers.GetDataTableColumns(data);

            Assert.IsType<ArrayList>(send);
        }
    }

    public class DataTableHelpers
    {
        public static ArrayList GetDataTableColumns(DataTable dataTable)
        {
            if (dataTable == null)
            {
                throw new ArgumentNullException(nameof(dataTable));
            }

            var columnsCount = dataTable.Columns.Count;
            var columnHeadings = new ArrayList();
            for (var i = 0; i < columnsCount; i++)
            {
                var dataColumn = dataTable.Columns[i];
                columnHeadings.Add(dataColumn.ColumnName.ToString());
            }
            return columnHeadings;
        }
    }
}
