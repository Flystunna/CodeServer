using CodeServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeServer.Data.Repositories
{
    public interface IProjectRepo : IRepository<project>
    {
        Task<IEnumerable<project>> GetAllProjectsAsync();
        Task<project> GetProjectByIdAsync(int id);
        Task<IEnumerable<project>> GetAllProjectBySdlcSystemIdAsync(int sdlcsystemid);
    }
}
