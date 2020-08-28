using CodeServer.Core.Models;
using CodeServer.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeServer.Data.Repositories
{
    public class SdlcSystemRepo : Repository<sdlc_system>, ISdlc_SystemRepo
    {
        public SdlcSystemRepo(ApplicationDbContext context)
            : base(context)
        { }

        public async Task<IEnumerable<sdlc_system>> GetAllSdlcSystemAsync()
        {
            return await ApplicationDbContext.sdlc_system.ToListAsync();
        }

        public async Task<sdlc_system> GetSdlcSystemByIdAsync(int id)
        {
            return await ApplicationDbContext.sdlc_system
                .SingleOrDefaultAsync(m => m.id == id);
        }

        private ApplicationDbContext ApplicationDbContext
        {
            get { return Context as ApplicationDbContext; }
        }
    }
}
