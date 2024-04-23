using System.Linq.Expressions;
using Infrastructure.Exceptions;
using Infrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

namespace Infrastructure.Query.Filters.ExpressionStrategy;

public class ExpressionContext(IExpressionOperation operation)
{
    public IExpressionOperation ExpressionOperation { get; set; } = operation;

    public Expression BuildExpression(Expression left, Expression right)
    {
        return ExpressionOperation.BuildExpression(left, right);
    }
}
