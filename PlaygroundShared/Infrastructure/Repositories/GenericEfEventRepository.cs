using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaygroundShared.Infrastructure.EF.EventDbContext;
using PlaygroundShared.Infrastructure.Events;

namespace PlaygroundShared.Infrastructure.Repositories
{
    public class GenericEfEventRepository<TEventEntity> : IGenericEventRepository<TEventEntity> where TEventEntity : BaseEventEntity
    {
        private readonly DbContext _dbContext;
        private DbSet<TEventEntity> Set { get; }

        public GenericEfEventRepository(EventDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Set = _dbContext.Set<TEventEntity>();
        }

        public async Task AddAsync(TEventEntity eventEntity)
        {
            await Set.AddAsync(eventEntity);
            await SaveAsync();
        }

        public async Task DeleteAsync(TEventEntity eventEntity)
        {
            Set.Remove(eventEntity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEventEntity>> GetForAggregateAsync(Guid aggregateId)
            => await Set.Where(x => x.AggregateId == aggregateId).ToListAsync();

        public async Task<TEventEntity> GetAsync(Guid id)
            => await Set.SingleOrDefaultAsync(x => x.Id == id);

        public async Task<IEnumerable<TEventEntity>> GetByCorrelationId(Guid correlationId)
            => await Set.Where(x => x.CorrelationId == correlationId).ToListAsync();
    }
}