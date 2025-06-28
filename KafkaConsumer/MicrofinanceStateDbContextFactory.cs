namespace KafkaConsumer;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class MicrofinanceStateDbContextFactory : IDesignTimeDbContextFactory<MicrofinanceStateDbContext>
{
    public MicrofinanceStateDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MicrofinanceStateDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=12345;Persist Security Info=True;Database=microfinance_state_database;Include Error Detail=true").UseSnakeCaseNamingConvention();
        return new MicrofinanceStateDbContext(optionsBuilder.Options);
    }
}
