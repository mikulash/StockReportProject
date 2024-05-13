using System.Linq.Expressions;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationNotEqual : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.NotEqual(left, right);
    }
}
