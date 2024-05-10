using System.Linq.Expressions;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationLessEqual : IExpressionOperation
{
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.LessThanOrEqual(left, right);
    }
}
