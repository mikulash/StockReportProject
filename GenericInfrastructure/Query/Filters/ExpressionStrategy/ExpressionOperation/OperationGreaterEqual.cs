using System.Linq.Expressions;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationGreaterEqual : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.GreaterThanOrEqual(left, right);
    }
}
