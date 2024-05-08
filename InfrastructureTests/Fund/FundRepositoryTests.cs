

using DataAccessLayer.Data;
using Microsoft.Extensions.DependencyInjection;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

namespace InfrastructureTests.Fund;

public class FundRepositoryTests
{
    private MockedDependencyInjectionBuilder _serviceProviderBuilder = null!;
    private IServiceScope _serviceScope = null!;

    [OneTimeSetUp]
    public void InitializeOnce()
    {
        _serviceProviderBuilder = new MockedDependencyInjectionBuilder()
            .AddDataAccessLayer(InfrastructureTestUtilities.CreateConfig())
            .AddInfrastructure();
    }

    [SetUp]
    public async Task Initialize()
    {
        _serviceScope = _serviceProviderBuilder.Create().CreateScope();

        var dbContext = _serviceScope.ServiceProvider.GetRequiredService<StockDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        await dbContext.Funds.AddRangeAsync(TestDataInitializer.GetTestFunds());
        await dbContext.SaveChangesAsync();
    }

    [TearDown]
    public void AfterEach()
    {
        _serviceScope.Dispose();
    }

    [Test]
    public async Task AddAsync_CreateCalled_NewEntityCreated()
    {
        // arrange
        var expected = TestDataInitializer.GetUncommittedTestFund();
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        await repository.AddAsync(expected);
        await uow.CommitAsync();
        
        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(all);
        Assert.GreaterOrEqual(all.Count, 1);
        Assert.NotNull(all.FirstOrDefault(fund => fund.FundName.Equals(expected.FundName)));
    }

    [Test]
    public async Task AddRangeAsync_CreateCalled_NewEntitiesCreated()
    {
        // arrange
        var expected = TestDataInitializer.GetUncommittedTestFunds();
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        await repository.AddRangeAsync(expected.ToArray());
        await uow.CommitAsync();

        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(all);
        Assert.GreaterOrEqual(all.Count, expected.Count);
        Assert.IsTrue(expected.All(ex => all.Contains(ex)));
    }

    [Test]
    public async Task Delete_EntityRemoved_RemoveCalled()
    {
        // arrange
        long entityId = 1L;
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        var entity = await repository.GetByIdAsync(entityId);
        Assert.NotNull(entity);
        repository.Delete(entity!);
        await uow.CommitAsync();
        
        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNull(all.FirstOrDefault(ent => ent.Id.Equals(entityId)));
    }

    [Test]
    public async Task DeleteRange_AllEntitiesRemoved_RemoveRangeCalled()
    {
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;
        
        var all = (await repository.GetAllAsync()).ToList();
        
        repository.DeleteRange(all.ToArray());
        await uow.CommitAsync();
        
        all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsEmpty(all);
    }

    [Test]
    public async Task Update_EntityUpdated_UpdateCalled()
    {
        // arrange
        var newFundName = "BetterFund";
        var entityId = 1L;
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        var toUpdate = await repository.GetByIdAsync(entityId);
        Assert.NotNull(toUpdate);

        toUpdate!.FundName = newFundName;
        repository.Update(toUpdate);
        await uow.CommitAsync();

        var actual = await repository.GetByIdAsync(entityId);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Id, Is.EqualTo(entityId));
        Assert.That(actual.FundName, Is.EqualTo(newFundName));
    }

    [Test]
    public async Task UpdateRange_EntitiesUpdated_UpdateRangeCalled()
    {
        // arrange
        var newFundName = "BetterFund";
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        var all = (await repository.GetAllAsync()).ToList();
        Assert.IsNotEmpty(all);
        
        all.ForEach(x => x.FundName = newFundName);
        repository.UpdateRange(all.ToArray());
        await uow.CommitAsync();
        
        all = (await repository.GetAllAsync()).ToList();
        // assert
        Assert.IsNotEmpty(all);
        Assert.IsTrue(all.All(x => x.FundName.Equals(newFundName)));
    }

    [Test]
    public async Task GetAllAsync_NoRestrictions_ReturnAllEntitiesFromDatabase()
    {
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(all);
        all.ForEach(x => Assert.NotNull(x.Id));
        all.ForEach(x => Assert.That(x.FundName, !Is.EqualTo(string.Empty)));
    }

    [Test]
    public async Task GetSingleAsync_NameRestriction_ReturnSingleEntity()
    {
        // arrange
        var fundName = "STARK";
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        var single = await repository.GetSingleAsync(x => x.FundName.Equals(fundName));
        
        // assert
        Assert.NotNull(single);
        Assert.That(single!.FundName, Is.EqualTo(fundName));
    }

    [Test]
    public async Task GetSingleAsync_NoEntityFound_ReturnsNull()
    {
        // arrange
        var fundName = "ThisShouldNotExist";
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        var single = await repository.GetSingleAsync(x => x.FundName.Equals(fundName));
        
        // assert
        Assert.IsNull(single);
    }

    [Test]
    public async Task GetByIdAsync_EntityFound_ReturnsEntityWithId()
    {
        // arrange
        var id = 1L;
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        var entity = await repository.GetByIdAsync(id);
        
        // assert
        Assert.NotNull(entity);
        Assert.That(entity!.Id, Is.EqualTo(id));
    }

    [Test]
    public async Task GetByIdAsync_EntityNitFound_ReturnsNull()
    {
        // arrange
        var id = 10000000L;
        
        // act
        using var uow = _serviceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.FundRepository;

        var entity = await repository.GetByIdAsync(id);
        
        // assert
        Assert.IsNull(entity);
    }
}
