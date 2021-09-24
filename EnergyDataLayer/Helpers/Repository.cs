using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EnergyDataLayer.Helpers
{
    public class Repository<TEntity> where TEntity : class
    {
        private DbContext _context;

        public Repository(DbContext context)
        {
            _context = context;
        }

        public TEntity Add(TEntity entity)
        {
            if (TryAttach(entity))
            {
                return entity;
            }

            return null;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            var addedCollection = new List<TEntity>();

            foreach (var entity in entities)
            {
                if (TryAttach(entity))
                {
                    addedCollection.Add(entity);
                }
            }

            return addedCollection;
        }

        private bool TryAttach(TEntity entity)
        {
            var entityFromDb = _context.Set<TEntity>()
                .Find(_context.GetPrimaryKeyValues(entity).ToArray());

            if (entityFromDb == null)
            {
                _context.Set<TEntity>().Add(entity);
                return true;
            }

            return false;
        }

        // We don't want to implement IDisposable in this Repository because .Net core's
        // dbcontext has been setup as 'scoped' meaning it will dispose after the request
        // and when we dispose manually e.g. in a repository the context will have been 
        // disposed..
    }
}
