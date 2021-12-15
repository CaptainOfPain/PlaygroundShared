using Microsoft.EntityFrameworkCore;

namespace PlaygroundShared.Infrastructure.EF.Contexts
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