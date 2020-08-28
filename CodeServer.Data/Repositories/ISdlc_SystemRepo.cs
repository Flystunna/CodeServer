using CodeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeServer.Data.Repositories
{
    public interface ISdlc_SystemRepo : IRepository<sdlc_system>
    {
        Task<IEnumerable<sdlc_system>> GetAllSdlcSystemAsync();
        Task<sdlc_system> GetSdlcSystemByIdAsync(int id);
    }
}
