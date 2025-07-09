namespace SimulateCredit.Infrastructure.Settings;

public sealed class MongoDbSettings
{
    public string ConnectionString { get; set; } = default!;
    public string Database { get; set; } = default!;
}
