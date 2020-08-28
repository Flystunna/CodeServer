using System;
using CodeServer.Core.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeServer.Data.Repositories;
using Serilog;
using Newtonsoft.Json;
using CodeServer.Data.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CodeServer.Business.Services
{
    public interface ISdlcSystemService
    {
        Task<IEnumerable<sdlc_system>> GetAllSdlcSystem();
        Task<sdlc_system> GetSdlcSystemById(int id);
        Task<sdlc_system> CreateSdlcSystem(sdlc_system model);
        Task UpdateSdlcSystem(int id, sdlc_system model);
        Task DeleteSdlcSystem(int id);
        Task<sdlc_system> GetSdlcSystemByIdAsNoTracking(int id);
    }
    public class SdlcSystemService : ISdlcSystemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public SdlcSystemService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            this._unitOfWork = unitOfWork;
            this._context = context;
        }
        public async Task<sdlc_system> CreateSdlcSystem(sdlc_system model)
        {
            if (model.base_url == null)
                throw new ArgumentNullException("Base URL cannot be null");

            model.created_date = DateTime.Now;
            model.last_modified_date = DateTime.Now;

            await _unitOfWork.Sdlc_Systems.AddAsync(model);

            Log.Information("Creation of new Sdlc System " + JsonConvert.SerializeObject(model));

            return model;
        }
        public async Task<IEnumerable<sdlc_system>> GetAllSdlcSystem()
        {
            var listOfAll = await _unitOfWork.Sdlc_Systems.GetAllSdlcSystemAsync();

            return listOfAll;
        }
        public async Task<sdlc_system> GetSdlcSystemByIdAsNoTracking(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("ID cannot be null");

            var sdlcSystem = await _context.sdlc_system.Where(c => c.id == id).AsNoTracking().FirstOrDefaultAsync();

            Log.Information("Get By Id Check for Sdlc System with id " + id);

            return sdlcSystem;
        }
        public async Task<sdlc_system> GetSdlcSystemById(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("ID cannot be null");

            var sdlcSystem = await _unitOfWork.Sdlc_Systems.GetSdlcSystemByIdAsync(id);

            Log.Information("Get By Id Check for Sdlc System with id " + id);

            return sdlcSystem;
        }
        public async Task UpdateSdlcSystem(int id, sdlc_system model)
        {
            if (id == 0)
                throw new ArgumentNullException("ID cannot be null");

            var ExistingSys = await GetSdlcSystemById(id);

            if (ExistingSys != null)
            {
                ExistingSys.last_modified_date = DateTime.Now;
                ExistingSys.description = model.description ?? ExistingSys.description;
                ExistingSys.base_url = model.base_url ?? ExistingSys.base_url;

                await _unitOfWork.SaveChangesAsync();
                Log.Information("Update for Sdlc System with Id " + id);
            }
            else
                throw new NullReferenceException("Sdlc System not found");

            await _unitOfWork.SaveChangesAsync();
        }
        public async Task DeleteSdlcSystem(int id)
        {
            if(id == 0)
                throw new ArgumentNullException("ID cannot be null");

            var GetById = await GetSdlcSystemById(id);

            if (GetById != null)
            {
                _unitOfWork.Sdlc_Systems.Remove(GetById);
                await _unitOfWork.SaveChangesAsync();
                Log.Information("Deletion of Sdlc System with Id " + id);
            }
            else
                throw new NullReferenceException("Sdlc System not found");
        }
    }
}
