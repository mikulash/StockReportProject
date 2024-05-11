using Microsoft.Extensions.DependencyInjection;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

namespace StockInfrastructureTests.Fund;

public class FundRepositoryTests : InfrastructureBaseTest
{
    [Test]
    public async Task AddAsync_CreateCalled_NewEntityCreated()
    {
        // arrange
        var expected = TestDataInitializer.GetUncommittedTestCompany();
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

        await repository.AddAsync(expected);
        await uow.CommitAsync();
        
        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(all);
        Assert.GreaterOrEqual(all.Count, 1);
        Assert.NotNull(all.FirstOrDefault(comp => comp.CompanyName.Equals(expected.CompanyName)));
    }

    [Test]
    public async Task AddRangeAsync_CreateCalled_NewEntitiesCreated()
    {
        // arrange
        var expected = TestDataInitializer.GetUncommittedTestCompanies();
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

        await repository.AddRangeAsync(expected.ToArray());
        await uow.CommitAsync();

        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(all);
        Assert.GreaterOrEqual(all.Count, expected.Count);
        Assert.IsTrue(expected.TrueForAll(ex => all.Contains(ex)));
    }

    [Test]
    public async Task Delete_EntityRemoved_RemoveCalled()
    {
        // arrange
        long entityId = 1L;
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

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
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;
        
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
        var newCompanyName = "NewCompanyName";
        var newCompanyTicker = "NewTICKER";
        var entityId = 1L;
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

        var toUpdate = await repository.GetByIdAsync(entityId);
        Assert.NotNull(toUpdate);

        toUpdate!.CompanyName = newCompanyName;
        toUpdate!.Ticker = newCompanyTicker;
        repository.Update(toUpdate);
        await uow.CommitAsync();

        var actual = await repository.GetByIdAsync(entityId);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual.Id, Is.EqualTo(entityId));
        Assert.That(actual.CompanyName, Is.EqualTo(newCompanyName));
        Assert.That(actual.Ticker, Is.EqualTo(newCompanyTicker));
    }

    [Test]
    public async Task UpdateRange_EntitiesUpdated_UpdateRangeCalled()
    {
        // arrange
        var newCompanyNames = new List<string>(capacity: TestDataInitializer.GetTestCompanies().Count);
        for (int index = 0; index < newCompanyNames.Capacity; index++)
        {
            newCompanyNames.Add(new Guid().ToString());
        }
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

        var all = (await repository.GetAllAsync()).ToList();
        Assert.IsNotEmpty(all);
        
        for (var index = 0; index < all.Count; index++)
        {
            all.ElementAt(index).CompanyName = newCompanyNames.ElementAt(index);
        }
        
        repository.UpdateRange(all.ToArray());
        await uow.CommitAsync();
        
        all = (await repository.GetAllAsync()).ToList();
        // assert
        Assert.IsNotEmpty(all);
        Assert.IsTrue(all.TrueForAll(x => newCompanyNames.Contains(x.CompanyName)));
    }

    [Test]
    public async Task GetAllAsync_NoRestrictions_ReturnAllEntitiesFromDatabase()
    {
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(all);
        all.ForEach(x => Assert.NotNull(x.Id));
        all.ForEach(x => Assert.That(x.CompanyName, !Is.EqualTo(string.Empty)));
    }

    [Test]
    public async Task GetSingleAsync_NameRestriction_ReturnSingleEntity()
    {
        // arrange
        var companyName = "TESLA INC";
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

        var single = await repository
            .GetSingleAsync(x => x.CompanyName.Equals(companyName), 
                x => x.IndexRecords);
        
        // assert
        Assert.NotNull(single);
        Assert.That(single!.CompanyName, Is.EqualTo(companyName));
        Assert.NotNull(single!.IndexRecords);
        Assert.IsNotEmpty(single!.IndexRecords!);
    }

    [Test]
    public async Task GetSingleAsync_NoEntityFound_ReturnsNull()
    {
        // arrange
        var companyName = "ThisShouldNotExist";
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

        var single = await repository.GetSingleAsync(x => x.CompanyName.Equals(companyName));
        
        // assert
        Assert.IsNull(single);
    }

    [Test]
    public async Task GetByIdAsync_EntityFound_ReturnsEntityWithId()
    {
        // arrange
        var id = 1L;
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

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
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.CompanyRepository;

        var entity = await repository.GetByIdAsync(id);
        
        // assert
        Assert.IsNull(entity);
    }
}
