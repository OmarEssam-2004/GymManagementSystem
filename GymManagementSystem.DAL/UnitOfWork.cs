using GymManagementSystem.DAL.Models;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly GymDbContext _context;
        private readonly ISessionRepository _SessionRepository;
        private readonly Dictionary<string, object> _repositories = [];


        public UnitOfWork(GymDbContext context, ISessionRepository sessionRepository)
        {
            _context = context;
            _SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository => _SessionRepository;

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var typeName = typeof(TEntity).Name;

            if (_repositories.TryGetValue(typeName, out object? value))
            { 
                return(value as IGenericRepository<TEntity>)!;
            }

            var repo = new GenericRepository<TEntity>(_context);
            _repositories.Add(typeName, repo);

            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await _context.SaveChangesAsync(ct);

    }
}
