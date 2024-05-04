﻿using System.Linq.Expressions;

namespace Infrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

public class OperationStringMethods : IExpressionOperation
{
    private readonly string _stringMethodName;

    public OperationStringMethods(string stringMethod)
    {
        _stringMethodName = stringMethod;
    }
    public Expression BuildExpression(Expression left, Expression right)
    {
        return Expression.Call(left, _stringMethodName, Type.EmptyTypes, right);
    }
}
