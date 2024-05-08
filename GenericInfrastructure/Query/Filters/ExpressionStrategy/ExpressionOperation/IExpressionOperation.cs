using System.Linq.Expressions;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public interface IExpressionOperation
{
    Expression BuildExpression(Expression left, Expression right);
}
