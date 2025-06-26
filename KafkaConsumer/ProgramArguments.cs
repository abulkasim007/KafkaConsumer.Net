namespace KafkaConsumer;

public class ProgramArguments(string kafkaServers, string[] postgreSqlParameters)
{
    public string KafkaServers { get; } = kafkaServers;
    public string PostgreSQLServer { get; } = string.Format("Host={0};Port={1};Username={2};Password={3};Persist Security Info=True;Database=microfinance_state_database;No Reset On Close=true", postgreSqlParameters[0], postgreSqlParameters[1], postgreSqlParameters[2], postgreSqlParameters[3]);
}