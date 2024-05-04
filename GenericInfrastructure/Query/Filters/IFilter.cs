using System.Linq.Expressions;

namespace GenericInfrastructure.Query.Filters;

public interface IFilter<TEntity> where TEntity : class
{
    Expression<Func<TEntity, bool>>? CreateExpression();
}
