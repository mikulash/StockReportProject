using System.Linq.Expressions;
using Infrastructure.Exceptions;
using Infrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

namespace Infrastructure.Query.Filters.ExpressionStrategy;

public class ExpressionContext
{
    public IExpressionOperation ExpressionOperation { get; set; }

    protected static IExpressionOperation ConvertOperation(Operation operation)
    {
        switch (operation)
        {
            case Operation.EQ:
                return new OperationEqual();
            case Operation.NE:
                return new OperationNotEqual();
            case Operation.LT:
                return new OperationLess();
            case Operation.LE:
                return new OperationLessEqual();
            case Operation.GT:
                return new OperationGreater();
            case Operation.GE:
                return new OperationGreaterEqual();
            case Operation.CONTAINS 
            or Operation.STARTSWITH:
                return new OperationStringMethods(operation.ToString());
            default:
                return new OperationEqual();
        }
    }

    public ExpressionContext(string operation)
    {
        if (!Enum.TryParse(operation, out Operation op))
        {
            throw new UnsupportedOperationExpressionException(operation);
        }

        ExpressionOperation = ConvertOperation(op);
    }

    public Expression BuildExpression(Expression left, Expression right)
    {
        return ExpressionOperation.BuildExpression(left, right);
    }
}
