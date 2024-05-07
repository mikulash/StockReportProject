using GenericDataAccessLayer.DatabaseTypeStrategy;
using GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;
using Microsoft.Extensions.Configuration;
using Moq;

namespace InfrastructureTests.Fund;

public class FundRepositoryTests
{
    public IConfiguration CreateConfig()
    {
        var mockDbContext = new Mock<IConfiguration>();
        mockDbContext
            .Setup(mock => mock.GetSection(EnumToStrategyConverter.AppSettingsKeyName).Value)
            .Returns(DatabaseType.InMemory.ToString());
        
        return mockDbContext.Object;
    }
}
