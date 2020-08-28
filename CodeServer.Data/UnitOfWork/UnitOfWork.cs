using CodeServer.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeServer.Data.Repositories
{
    public interface IIdentifier
    {
        public int Id { get; set; }

        bool IsNull();
    }
    public static class UOFExtensions
    {
        public static void DetachLocal<T>(this DbContext context, T t, int entryId) where T : class, IIdentifier
        {
            var local = context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));
            if (!local.IsNull())
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(t).State = EntityState.Modified;
        }
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private ProjectRepo _projectRepository;
        private SdlcSystemRepo _sdlcSystemRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IProjectRepo Projects => _projectRepository = _projectRepository ?? new ProjectRepo(_context);
        public ISdlc_SystemRepo Sdlc_Systems => _sdlcSystemRepository = _sdlcSystemRepository ?? new SdlcSystemRepo(_context);
        public void BeginTransaction()
        {
            _context.ChangeTracker.AutoDetectChangesEnabled = false;

            if (_context.Database.GetDbConnection().State != ConnectionState.Open)
                _context.Database.OpenConnection();

            _context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _context.ChangeTracker.DetectChanges();
            SaveChanges();
            _context.Database.CurrentTransaction.Commit();
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        public void Rollback()
        {
            _context.Database.CurrentTransaction?.Rollback();
        }
    }
}
