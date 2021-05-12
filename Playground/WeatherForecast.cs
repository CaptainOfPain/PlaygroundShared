using System;
using Microsoft.EntityFrameworkCore;
using PlaygroundShared.Domain;
using PlaygroundShared.DomainEvents;

namespace Playground
{
    public class WeatherForecast : IDomainEvent
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; set; }
        public AggregateId Id { get; set; }
    }

    public class TestDbContext : DbContext
    {
        protected TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}