using System.Linq.Expressions;

namespace Infrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationEqual : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.Equal(left, right);
    }
}
