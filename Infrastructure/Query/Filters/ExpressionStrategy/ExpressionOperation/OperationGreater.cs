using System.Linq.Expressions;

namespace Infrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationGreater : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.GreaterThan(left, right);
    }
}
