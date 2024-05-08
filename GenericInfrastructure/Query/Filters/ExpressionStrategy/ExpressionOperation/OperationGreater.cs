using System.Linq.Expressions;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationGreater : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.GreaterThan(left, right);
    }
}
