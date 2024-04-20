using System.Linq.Expressions;

namespace Infrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationLess : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.LessThan(left, right);
    }
}
