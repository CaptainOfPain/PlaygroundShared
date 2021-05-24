using Microsoft.EntityFrameworkCore;

namespace PlaygroundShared.Infrastructure.EF.EventDbContext
{
    public abstract class EventDbContext : DbContext
    {
        protected EventDbContext()
        {
        }

        protected EventDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}