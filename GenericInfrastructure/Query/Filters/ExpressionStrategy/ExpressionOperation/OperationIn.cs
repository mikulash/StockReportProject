using System.Linq.Expressions;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationIn : IExpressionOperation
{
    private const string StringMethodName = "Contains";

    public Expression BuildExpression(Expression left, Expression right)
    {
        // here we need to invert left and right since for collections we want x => coll.Contains(x)
        return Expression.Call(right, StringMethodName, Type.EmptyTypes, left);
    }
}
