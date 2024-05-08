using GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;
using Microsoft.Extensions.Configuration;

namespace GenericDataAccessLayer.DatabaseTypeStrategy;

public static class EnumToStrategyConverter
{
    private const string AppSettingsKeyName = "DatabaseType";

    public static IDBStrategy CreateStrategy(IConfiguration config)
    {
        if (!Enum.TryParse(config.GetSection(AppSettingsKeyName).Value ?? string.Empty, out DatabaseType dbType))
        {
            throw new UnsupportedDatabaseTypeException(dbType);
        }

        return dbType switch
        {
            DatabaseType.SQLite => new SQLiteDbStrategy(config),
            _ => throw new UnsupportedDatabaseTypeException(dbType)
        };
    }
}