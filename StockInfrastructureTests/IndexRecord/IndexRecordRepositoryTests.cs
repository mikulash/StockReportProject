using Microsoft.Extensions.DependencyInjection;
using StockInfrastructure.UnitOfWork;
using TestUtilities;

namespace StockInfrastructureTests.IndexRecord;

public class IndexRecordRepositoryTests : InfrastructureBaseTest
{
    [Test]
    public async Task AddAsync_CreateCalled_NewEntityCreated()
    {
        // arrange
        var expected = TestDataInitializer.GetUncommittedTestIndexRecord();
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.IndexRecordRepository;

        await repository.AddAsync(expected);
        await uow.CommitAsync();
        
        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(all);
        Assert.GreaterOrEqual(all.Count, 1);
        Assert.NotNull(all.FirstOrDefault(rec => rec.CompanyId.Equals(expected.CompanyId)));
        Assert.NotNull(all.FirstOrDefault(rec => rec.FundId.Equals(expected.FundId)));
    }

    [Test]
    public async Task AddRangeAsync_CreateCalled_NewEntitiesCreated()
    {
        // arrange
        var expected = TestDataInitializer.GetUncommittedTestIndexRecords();
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.IndexRecordRepository;

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
        var repository = uow.IndexRecordRepository;

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
        var repository = uow.IndexRecordRepository;
        
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
        var newDate = new DateOnly(2024, 5, 5);
        var entityId = 1L;
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.IndexRecordRepository;

        var toUpdate = await repository.GetByIdAsync(entityId);
        Assert.NotNull(toUpdate);

        toUpdate!.IssueDate = newDate;
        repository.Update(toUpdate);
        await uow.CommitAsync();

        var actual = await repository.GetByIdAsync(entityId);
        
        // assert
        Assert.NotNull(actual);
        Assert.That(actual!.Id, Is.EqualTo(entityId));
        Assert.That(actual.IssueDate, Is.EqualTo(newDate));
    }

    [Test]
    public async Task UpdateRange_EntitiesUpdated_UpdateRangeCalled()
    {
        // arrange
        var newDate = new DateOnly(2024, 5, 5);
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.IndexRecordRepository;

        var all = (await repository.GetAllAsync()).ToList();
        Assert.IsNotEmpty(all);
        
        all.ForEach(x => x.IssueDate = newDate);
        repository.UpdateRange(all.ToArray());
        await uow.CommitAsync();
        
        all = (await repository.GetAllAsync()).ToList();
        // assert
        Assert.IsNotEmpty(all);
        Assert.IsTrue(all.All(x => x.IssueDate.Equals(newDate)));
    }

    [Test]
    public async Task GetAllAsync_NoRestrictions_ReturnAllEntitiesFromDatabase()
    {
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.IndexRecordRepository;

        var all = (await repository.GetAllAsync()).ToList();
        
        // assert
        Assert.IsNotEmpty(all);
        all.ForEach(x => Assert.NotNull(x.Id));
        all.ForEach(x => Assert.That(x.CompanyId, !Is.EqualTo(0)));
        all.ForEach(x => Assert.That(x.FundId, !Is.EqualTo(0)));
    }

    [Test]
    public async Task GetSingleAsync_NameRestriction_ReturnSingleEntity()
    {
        // arrange
        var companyId = 1L;
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.IndexRecordRepository;

        var single = 
            await repository.GetSingleAsync(x => x.CompanyId.Equals(companyId),
                x => x.Company!);
        
        // assert
        Assert.NotNull(single);
        Assert.That(single!.CompanyId, Is.EqualTo(companyId));
        Assert.NotNull(single.Company);
        Assert.That(single.Company!.Id, Is.EqualTo(companyId));
    }

    [Test]
    public async Task GetSingleAsync_NoEntityFound_ReturnsNull()
    {
        // arrange
        var companyId = 1000000L;
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.IndexRecordRepository;

        var single = await repository.GetSingleAsync(x => x.CompanyId.Equals(companyId));
        
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
        var repository = uow.IndexRecordRepository;

        var entity = await repository.GetByIdAsync(id, 
            x => x.Company!, 
            x => x.Fund!);
        
        // assert
        Assert.NotNull(entity);
        Assert.That(entity!.Id, Is.EqualTo(id));
        Assert.NotNull(entity.Company);
        Assert.NotNull(entity.Fund);
    }

    [Test]
    public async Task GetByIdAsync_EntityNitFound_ReturnsNull()
    {
        // arrange
        var id = 10000000L;
        
        // act
        using var uow = ServiceScope.ServiceProvider.GetRequiredService<IStockUnitOfWork>();
        var repository = uow.IndexRecordRepository;

        var entity = await repository.GetByIdAsync(id);
        
        // assert
        Assert.IsNull(entity);
    }
}
