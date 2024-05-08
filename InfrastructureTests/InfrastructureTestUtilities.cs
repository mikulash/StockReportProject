using GenericDataAccessLayer.DatabaseTypeStrategy;
using GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;
using Microsoft.Extensions.Configuration;
using Moq;

namespace InfrastructureTests;

public static class InfrastructureTestUtilities
{
    private const string AppSettingsInMemoryDatabaseKey = "InMemDatabaseName";
    private const string AppSettingsInMemoryDatabaseName = "StockWebAPIDatabase";
    
    public static IConfiguration CreateConfig()
    {
        var mockConfig = new Mock<IConfiguration>();
        mockConfig
            .Setup(mock => mock.GetSection(EnumToStrategyConverter.AppSettingsKeyName).Value)
            .Returns(DatabaseType.InMemory.ToString());

        mockConfig
            .Setup(mock => mock.GetSection(AppSettingsInMemoryDatabaseKey).Value)
            .Returns(AppSettingsInMemoryDatabaseName);;
        
        return mockConfig.Object;
    }
}
