using CodeServer.Core.Models;
using CodeServer.Data.Data;
using CodeServer.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeServer.Business.Services
{

    public interface IProjectService
    {
        Task<IEnumerable<project>> GetAllProjects();
        Task<project> GetProjectById(int id);
        Task<IEnumerable<project>> GetProjectBySdlcSystemId(int id);
        Task<project> CreateProject(project model);
        Task<project> UpdateProject(int id, project model);
        Task DeleteProject(int id);
        Task<project> GetProjectByIdAsNoTracking(int id);
    }
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
       
        public ProjectService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            this._unitOfWork = unitOfWork;
            this._context = context;
        }
        public async Task<project> CreateProject(project model)
        {
            if (model.sdlc_system.id == 0)
                throw new NullReferenceException("Sdlc System Id cannot be null");
            if (model.external_id == null)
                throw new NullReferenceException("External Id cannot be null");

            //re map
            var newModel = new project
            {
                created_date = DateTime.Now,
                last_modified_date = DateTime.Now,
                external_id = model.external_id,
                sdlc_systemid = model.sdlc_system.id,
                name = model.name,
                
            };
            await _unitOfWork.Projects.AddAsync(newModel);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
            return newModel;         
        }

        public async Task<IEnumerable<project>> GetAllProjects()
        {
            var listOfAll = await _unitOfWork.Projects.GetAllProjectsAsync();

            return listOfAll;

        }

        public async Task<project> GetProjectById(int id)
        {
            if (id == 0)
                throw new NullReferenceException("ID cannot be null");

            var project = await _unitOfWork.Projects.GetProjectByIdAsync(id);
            return project;
        }
        public async Task<project> GetProjectByIdAsNoTracking(int id)
        {
            if (id == 0)
                throw new NullReferenceException("ID cannot be null");

            var project = await _context.project.Where(c => c.id == id).AsNoTracking().FirstOrDefaultAsync();
            return project;
        }

        public async Task<IEnumerable<project>> GetProjectBySdlcSystemId(int id)
        {
            if (id == 0)
                throw new NullReferenceException("ID cannot be null");

            var projectBySdlcSystem = await _unitOfWork.Projects.GetAllProjectBySdlcSystemIdAsync(id);

            return projectBySdlcSystem;
        }

        public async Task<project> UpdateProject(int id, project model)
        {
            if (id == 0)
                throw new NullReferenceException("ID cannot be null");

            var ExistingProject = await GetProjectById(id);

            if (ExistingProject != null)
            {
                ExistingProject.last_modified_date = DateTime.Now;
                ExistingProject.name = model.name ?? ExistingProject.name;
                ExistingProject.external_id = model.external_id ?? ExistingProject.external_id;
                if(model.sdlc_system != null)
                {
                    if(model.sdlc_system.id != 0)
                        ExistingProject.sdlc_systemid = model.sdlc_system.id != 0 ? model.sdlc_system.id : ExistingProject.sdlc_system.id;
                }
               
                try
                {
                    await _context.SaveChangesAsync();
                    return ExistingProject;
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    return null;
                }
            }
            else
                throw new NullReferenceException("Project not found");
            
        }
        public async Task DeleteProject(int id)
        {
            if (id == 0)
                throw new NullReferenceException("ID cannot be null");

            var ExistingProject = await GetProjectById(id);
            if(ExistingProject != null)
                _unitOfWork.Projects.Remove(ExistingProject);
            else
                throw new NullReferenceException("Project not found");
        }
    }
}
