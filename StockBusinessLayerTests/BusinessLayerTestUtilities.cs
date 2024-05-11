using System.Linq.Expressions;
using GenericDataAccessLayer.Models;
using GenericInfrastructure.Query;
using GenericInfrastructure.Repository;
using Moq;
using StockInfrastructure.UnitOfWork;

namespace StockBusinessLayerTests;

public static class BusinessLayerTestUtilities
{
    public static void InitializeUoW<TEntity>(Mock<IStockUnitOfWork> mockedUoW, Mock<IGenericRepository<TEntity, long>> mockedRepository, Mock<IQuery<TEntity, long>> mockedQuery) where TEntity : BaseEntity<long>
    {
        mockedUoW
            .Setup(mock => mock.GetRepositoryByEntity<TEntity, long>())
            .Returns(mockedRepository.Object);
        mockedUoW
            .Setup(mock => mock.GetQueryByEntity<TEntity, long>())
            .Returns(mockedQuery.Object);
        mockedUoW
            .Setup(mock => mock.CommitAsync())
            .Verifiable();
    }
    
    public static void InitializeQuery<TEntity>(Mock<IQuery<TEntity, long>> mockedQuery) where TEntity : BaseEntity<long>
    {
        mockedQuery
            .Setup(mock => mock.Reset())
            .Verifiable();

        mockedQuery
            .Setup(mock => mock.Where(It.IsAny<Expression<Func<TEntity, bool>>?>()))
            .Returns(mockedQuery.Object)
            .Verifiable();

        mockedQuery
            .Setup(mock => mock.Include(It.IsAny<Expression<Func<TEntity, object?>>[]>()))
            .Returns(mockedQuery.Object)
            .Verifiable();
        
        mockedQuery
            .Setup(mock => mock.Page(It.IsAny<int>(), It.IsAny<int>()))
            .Returns(mockedQuery.Object)
            .Verifiable();
        
        mockedQuery
            .Setup(mock => mock.SortBy(It.IsAny<string>(), It.IsAny<bool>()))
            .Returns(mockedQuery.Object)
            .Verifiable();
    }
    
    
}