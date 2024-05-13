using System.Linq.Expressions;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationEqual : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.Equal(left, right);
    }
}
