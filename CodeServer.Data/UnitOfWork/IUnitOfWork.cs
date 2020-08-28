using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CodeServer.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepo Projects { get; }
        ISdlc_SystemRepo Sdlc_Systems { get; }
        void SaveChanges();
        Task<int> SaveChangesAsync();
        void BeginTransaction();
        void Commit();
        void Rollback();

    }
}
