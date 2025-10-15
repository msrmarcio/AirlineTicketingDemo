using CustomerHistoryService.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CustomerHistoryService.Infrastructure.Persistence
{
    public class CustomerHistoryDbContext : DbContext
    {
        public DbSet<CustomerHistory> Histories { get; set; }

        public CustomerHistoryDbContext(DbContextOptions<CustomerHistoryDbContext> options)
            : base(options) { }
    }

}
