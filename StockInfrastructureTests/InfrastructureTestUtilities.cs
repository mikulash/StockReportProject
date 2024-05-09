using DataAccessLayer.Data;
using GenericDataAccessLayer.DatabaseTypeStrategy;
using GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestUtilities;

namespace StockInfrastructureTests;

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

    public static MockedDependencyInjectionBuilder CreateMockedDependencyInjectionBuilder() =>
        new MockedDependencyInjectionBuilder()
            .AddDataAccessLayer(CreateConfig())
            .AddInfrastructure();

    public static async Task<IServiceScope> CreateServiceScopeAndInitializeDatabaseAsync(MockedDependencyInjectionBuilder builder)
    {
        var serviceScope = builder.Create().CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<StockDbContext>();
        
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        
        await dbContext.Funds.AddRangeAsync(TestDataInitializer.GetTestFunds());
        await dbContext.Companies.AddRangeAsync(TestDataInitializer.GetTestCompanies());
        await dbContext.IndexRecords.AddRangeAsync(TestDataInitializer.GetTestIndexRecords());
        await dbContext.SaveChangesAsync();

        return serviceScope;
    }
}
