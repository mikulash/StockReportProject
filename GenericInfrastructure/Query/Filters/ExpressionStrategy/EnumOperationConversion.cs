using GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

namespace GenericInfrastructure.Query.Filters.ExpressionStrategy;

public static class EnumOperationConversion
{
    public static IExpressionOperation ConvertOperation(Operation operation)
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
            case Operation.IN:
                return new OperationIn();
            default:
                return new OperationEqual();
        }
    }
}
