using CodeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CodeServer.Data.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeServer.Data.Repositories
{
    public class ProjectRepo : Repository<project>, IProjectRepo
    {
        public ProjectRepo(ApplicationDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<project>> GetAllProjectBySdlcSystemIdAsync(int sdlcsystemid)
        {
            return await ApplicationDbContext.project
                .Include(m => m.sdlc_system)
                .Where(m => m.sdlc_systemid == sdlcsystemid)
                .ToListAsync();
        }

        public async Task<IEnumerable<project>> GetAllProjectsAsync()
        {
            return await ApplicationDbContext.project
               .Include(m => m.sdlc_system)
               .ToListAsync();
        }

        public async Task<project> GetProjectByIdAsync(int id)
        {
            return await ApplicationDbContext.project
                .Include(m => m.sdlc_system)
                .SingleOrDefaultAsync(m => m.id == id);
        }

        private ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
