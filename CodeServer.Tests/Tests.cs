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

namespace CodeServer.Tests
{
    public class UnitTest
    {
        private readonly IProjectService _projectSvc;
        private readonly ISdlcSystemService _sdlcSvc;
        private readonly IMapper _mapper;

        [Fact]
        public async void GetProjectWithInvalidPath()
        {
            //Arrange & Act
            ProjectsController projController = new ProjectsController(_projectSvc, _mapper, _sdlcSvc);

            var postId = "whatever";
            int val;
            bool result = int.TryParse(postId, out val);

            var data = await projController.GetProject(val);

            var okResult = data as Microsoft.AspNetCore.Mvc.StatusCodeResult;
            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }
        [Fact]
        public async void GetProjectIDWithValidID()
        {
            // Arrange & Act
            var mockProjectSvc = new Mock<IProjectService>();
            var mockSdlcSvc = new Mock<ISdlcSystemService>();
            var mockMapper = new Mock<IMapper>();

            var controller = new ProjectsController(mockProjectSvc.Object, mockMapper.Object, mockSdlcSvc.Object);
            // Act
            var result = await controller.GetProject(id: 1);

            ///Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async void PostPayloadNotContainingSystem()
        {
            //Arrange & Act
            ProjectsController projController = new ProjectsController(_projectSvc, _mapper, _sdlcSvc);

            var model = new ProjectDTO();
            model.external_id = "EXTERNAL-ID";
            var result = await projController.CreateProject(model);
            var okResult = result as Microsoft.AspNetCore.Mvc.StatusCodeResult;

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, okResult.StatusCode);
        }
        private project CreateDefault()
        {
            var model = new project
            {
                external_id = "SAMPLEPROJECT",

            };
            model.sdlc_system = new sdlc_system
            {
                id = 1
            };

            return model;
        }

        [Fact]
        public async void PostExpected201WithLocationHeader()
        {
            IProjectService prjServiceFake = A.Fake<IProjectService>();
            ISdlcSystemService sdlcServiceFake = A.Fake<ISdlcSystemService>();
            IMapper mapServiceFake = A.Fake<IMapper>();
            List<project> prjList = new List<project>{
                CreateDefault()
            };

            var modelDTO = new ProjectDTO
            {
                external_id = "EXTERNALID",
                name = "name"
            };
            modelDTO.sdlcSystem = new sdlcSystem
            {
                id = 1
            };
            #region comments
            //var model2 = new project
            //{
            //    external_id = "SAMPLEPROJECT",

            //};
            //model2.sdlc_system = new sdlc_system
            //{
            //    id = 1
            //};

            //var mockProjectSvc = new Mock<IProjectService>();
            //var mockSdlcSvc = new Mock<ISdlcSystemService>();
            //var mockUow = new Mock<IUnitOfWork>();
            //var mockMapper = new Mock<IMapper>();
            //var mockDbContext = new Mock<ApplicationDbContext>();

            //mockProjectSvc.Setup(x => x.CreateProject(It.IsAny<project>())).ReturnsAsync(model2);
            //mockSdlcSvc.Setup(x => x.GetSdlcSystemByIdAsNoTracking(It.IsAny<int>())).ReturnsAsync(model2.sdlc_system);

            //var controller = new ProjectsController(mockProjectSvc.Object, mockMapper.Object, mockSdlcSvc.Object);
            //controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            //controller.ControllerContext.HttpContext = new DefaultHttpContext();
            //controller.ControllerContext.HttpContext.Request.Scheme = "http";
            //controller.ControllerContext.HttpContext.Request.Host = new HostString("localhost");

            //var result1 = await controller.CreateProject(model: modelDTO);
            #endregion

            A.CallTo(() => prjServiceFake.GetAllProjects()).Returns(prjList);

            ProjectsController sut = CreateSut(prjServiceFake, mapServiceFake, sdlcServiceFake);

            sut.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            sut.ControllerContext.HttpContext = new DefaultHttpContext();
            sut.ControllerContext.HttpContext.Request.Scheme = "http";
            sut.ControllerContext.HttpContext.Request.Host = new HostString("localhost");
            //Act
            var result = await sut.CreateProject(modelDTO);

            //Assert
            Assert.IsType<CreatedResult>(result);
        }

        private ProjectsController CreateSut(IProjectService projServiceFake, IMapper map, ISdlcSystemService sdlcSystemService)
        {
            return new ProjectsController(projServiceFake, map, sdlcSystemService);
        }
    }

    public class IntegrationTest
    {
        private TestServer _server;
        public HttpClient _client { get; private set; }

        public IntegrationTest()
        {
            SetUpClient();
        }

        [Fact]
        private async void SetUpClient()
        {
            var hostBuilder = new HostBuilder()
           .ConfigureWebHost(webHost =>
           {
               // Add TestServer
               webHost.UseTestServer();
               webHost.Configure(app => app.Run(async ctx =>
                   await ctx.Response.WriteAsync("Code Server Api is running")));
           });

            // Build and start the IHost
            var host = await hostBuilder.StartAsync();

            // Create an HttpClient to send requests to the TestServer
            var client = host.GetTestClient();

            var response = await client.GetAsync("/");

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK.ToString(), response.StatusCode.ToString());
        }
    }
}
