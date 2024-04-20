using System.Linq.Expressions;

namespace Infrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationNotEqual : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.NotEqual(left, right);
    }
}
