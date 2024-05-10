using System.Linq.Expressions;
using GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy;

public class ExpressionContext(IExpressionOperation operation)
{
    public IExpressionOperation ExpressionOperation { get; set; } = operation;

    public Expression BuildExpression(Expression left, Expression right)
    {
        return ExpressionOperation.BuildExpression(left, right);
    }
}
