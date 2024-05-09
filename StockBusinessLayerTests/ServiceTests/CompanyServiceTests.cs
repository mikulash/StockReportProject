using System.Linq.Expressions;
using DataAccessLayer.Models;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

namespace StockBusinessLayerTests.ServiceTests;

public class CompanyServiceTests
{
    private MockedDependencyInjectionBuilder _serviceProviderBuilder = null!;

    private Mock<IGenericRepository<Company, long>> _mockedRepository = null!;
    private Mock<IQuery<Company, long>> _mockedQuery = null!;
    private Mock<IStockUnitOfWork> _mockedUoW = null!;
}