namespace KafkaConsumer;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

internal class DbContextProvider(ProgramArguments programArguments)
{
    //private const string DatabaseConnectionStringRemote = "Host=10.42.53.223;Port=5433;Username=postgres;Password=UAHSq248uwqejadkASJD;Persist Security Info=True;Database=microfinance_state_database;No Reset On Close=true";

    //private const string DatabaseConnectionStringRemote = "Host=localhost;Port=5432;Username=postgres;Password=12345;Persist Security Info=True;Database=microfinance_state_database;No Reset On Close=true";

    private readonly PooledDbContextFactory<MicrofinanceStateDbContext> dbContextFactory = CreateDbContextFactory(programArguments.PostgreSQLServer);

    public MicrofinanceStateDbContext Get()
    {
        return dbContextFactory.CreateDbContext();
    }

    private static PooledDbContextFactory<MicrofinanceStateDbContext> CreateDbContextFactory(string connectionString)
    {
        DbContextOptions<MicrofinanceStateDbContext> dbContextOptions = new DbContextOptionsBuilder<MicrofinanceStateDbContext>()
           .EnableThreadSafetyChecks(false)
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
           .UseSnakeCaseNamingConvention()
           .UseNpgsql(connectionString)
           .Options;

        return new PooledDbContextFactory<MicrofinanceStateDbContext>(dbContextOptions);
    }
}
