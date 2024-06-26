﻿using System.Linq.Expressions;
using GenericInfrastructure.Query.Filters.ExpressionStrategy;
using GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;
using GenericInfrastructure.Query.Filters.LambdaAndOr;

namespace GenericInfrastructure.Query.Filters;

public abstract class FilterBase<TEntity> : IFilter<TEntity> where TEntity : class
{
    private const string LambdaParam = "source";

    protected readonly IDictionary<string, Expression<Func<TEntity, bool>>> LambdaDictionary;
    
    public FilterBase()
    {
        LambdaDictionary = new Dictionary<string, Expression<Func<TEntity, bool>>>();
        SetUpSpecialLambdaExpressions();
    }

    protected virtual void SetUpSpecialLambdaExpressions()
    {
    }

    protected virtual Expression BuildExpression(Operation op, Expression left, Expression right) 
        => new ExpressionContext(
                EnumOperationConversion.ConvertOperation(op)
            )
            .BuildExpression(left, right);
    
    

    public virtual Expression<Func<TEntity, bool>>? CreateExpression()
    {
        Expression<Func<TEntity, bool>>? final = null;
        var param = Expression.Parameter(typeof(TEntity), LambdaParam);

        foreach (var item in GetType().GetProperties())
        {
            var constant = Expression.Constant(item.GetValue(this));
            if (constant.Value == null)
            {
                continue;
            }

            if (!LambdaDictionary.TryGetValue(item.Name, out Expression<Func<TEntity, bool>>? current))
            {
                var filterParams = new PropertyFilterParameters();
                filterParams.ParsePropertyName(item.Name);
                Expression expression = BuildExpression(filterParams.Operation, filterParams.TryCreateMember(param), constant);
                current = Expression.Lambda<Func<TEntity, bool>>(expression, param);
            }

            final = final == null ? current : final.And(current);
        }
        return final;
    }
}
