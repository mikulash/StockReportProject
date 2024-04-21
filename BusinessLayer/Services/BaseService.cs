using Infrastructure.UnitOfWork;

namespace BusinessLayer.Services;

public abstract class BaseService(IUnitOfWork unitOfWork) : IBaseService
{
    protected readonly IUnitOfWork UnitOfWork = unitOfWork;

    public virtual async Task SaveAsync(bool save = true)
    {
        if (save)
        {
            await UnitOfWork.CommitAsync();
        }
    }
}
