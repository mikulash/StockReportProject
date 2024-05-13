using GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;
using Microsoft.Extensions.Configuration;

namespace GenericDataAccessLayer.DatabaseTypeStrategy;

public static class EnumToStrategyConverter
{
    public const string AppSettingsKeyName = "DatabaseType";

    public static BaseDbStrategy CreateStrategy(IConfiguration config)
    {
        if (!Enum.TryParse(config.GetSection(AppSettingsKeyName).Value ?? string.Empty, out DatabaseType dbType))
        {
            throw new UnsupportedDatabaseTypeException(dbType);
        }

        return dbType switch
        {
            DatabaseType.SQLite => new SqLiteDbStrategy(config),
            DatabaseType.InMemory => new InMemoryDbStrategy(config),
            _ => throw new UnsupportedDatabaseTypeException(dbType)
        };
    }
}