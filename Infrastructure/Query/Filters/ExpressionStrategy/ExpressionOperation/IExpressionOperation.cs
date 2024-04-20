using System.Linq.Expressions;

namespace Infrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public interface IExpressionOperation
{
    Expression BuildExpression(Expression left, Expression right);
}
