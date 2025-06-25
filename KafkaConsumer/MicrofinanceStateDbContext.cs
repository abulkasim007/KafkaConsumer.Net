namespace KafkaConsumer;

using Microsoft.EntityFrameworkCore;

public class MicrofinanceStateDbContext(DbContextOptions<MicrofinanceStateDbContext> options) : DbContext(options)
{
    public DbSet<LoanAggregateRoot> LoanAggregateRoots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
           .Entity<LoanAggregateRoot>()
           .ToTable("loan_aggregate_roots");

        base.OnModelCreating(modelBuilder);
    }
}
