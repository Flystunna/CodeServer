using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CodeServer.Business.Services;
using CodeServer.Core.Models;
using CodeServer.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;

namespace CodeServer.Controllers
{
    [Route("api/v2/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        public IProjectService _projectSvc;
        public ISdlcSystemService _sdlSvc;
        private readonly IMapper _mapper;
        public ProjectsController(IProjectService projectSvc, IMapper mapper, ISdlcSystemService sdlSvc)
        {
            _projectSvc = projectSvc;
            _mapper = mapper;
            _sdlSvc = sdlSvc;
        }
        [HttpGet]
        [Route("GetAllProjects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                var getAll = await _projectSvc.GetAllProjects();

                var getAllVM = _mapper.Map<IEnumerable<project>, IEnumerable<ProjectDTO>>(getAll);

                return Ok(getAllVM);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred " + ex);
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProject(int id)
        {
            try
            {
                var getProject = await _projectSvc.GetProjectById(id);

                var getProjectVM = _mapper.Map<project, ProjectDTO>(getProject);
                return Ok(getProjectVM);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred " + ex);
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject(ProjectDTO model)
        {
            try
            {
                var idd = model.sdlcSystem.id;
                var sdlcSys = await _sdlSvc.GetSdlcSystemByIdAsNoTracking(idd);

                if (sdlcSys == null)
                    return NotFound();

                var vm = _mapper.Map<ProjectDTO, project>(model);         

                var addProject = await _projectSvc.CreateProject(vm);

                if (addProject == null)
                    return Conflict();

                return Created(new Uri(Request.GetEncodedUrl() + "/"), addProject);
            }
            catch (Exception ex)
            {
                Log.Error("An error occurred " + ex);
                return BadRequest();
            }
        }
        [HttpPatch]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult> PatchProject(int id, ProjectDTO patchDoc)
        {
            try
            {
                var getProject = await _projectSvc.GetProjectByIdAsNoTracking(id);

                if (getProject == null)
                {
                    return NotFound();
                }

                if (patchDoc.sdlcSystem != null)
                {
                    if (patchDoc.sdlcSystem.id != 0)
                    {
                        var sdlcSys = await _sdlSvc.GetSdlcSystemByIdAsNoTracking(patchDoc.sdlcSystem.id);

                        if (sdlcSys == null)
                            return NotFound();
                    }
                }

                if (patchDoc.name == null && patchDoc.external_id == null && patchDoc.sdlcSystem == null)
                {
                    return NotFound();
                } 
                
                var project = new project();

                var toUpdate = _mapper.Map(patchDoc, project);

                var newUpdate = await _projectSvc.UpdateProject(id, toUpdate);
                if(newUpdate ==null)
                    return Conflict();

                return Ok(newUpdate);
            }
            catch(Exception ex)
            {
                Log.Error("An error occurred " + ex);
                return BadRequest();
            }
        }
    }
}
