using GenericInfrastructure.UnitOfWork;

namespace GenericBusinessLayer.Services;

public abstract class BaseService<TUnitOfWork>(TUnitOfWork unitOfWork) : IBaseService
    where TUnitOfWork : IBaseUnitOfWork
{
    protected readonly TUnitOfWork UnitOfWork = unitOfWork;

    public virtual async Task SaveAsync(bool save = true)
    {
        if (save)
        {
            await UnitOfWork.CommitAsync();
        }
    }
}
