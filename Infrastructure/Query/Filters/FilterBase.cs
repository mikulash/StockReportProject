using System.Linq.Expressions;
using Infrastructure.Exceptions;
using Infrastructure.Query.Filters.ExpressionStrategy;
using Infrastructure.Query.Filters.LambdaAndOr;

namespace Infrastructure.Query.Filters;

public abstract class FilterBase<TEntity> : IFilter<TEntity> where TEntity : class
{
    private const string LambdaParam = "source";
    private const char SeparatorCharacter = '_';
    private const string DefaultOperation = "EQ";

    protected readonly IDictionary<string, Expression<Func<TEntity, bool>>> LambdaDictionary;
    
    public FilterBase()
    {
        LambdaDictionary = new Dictionary<string, Expression<Func<TEntity, bool>>>();
        SetUpSpecialLambdaExpressions();
    }

    protected abstract void SetUpSpecialLambdaExpressions();

    protected virtual Expression BuildExpression(string? op, Expression left, Expression right) 
        => new ExpressionContext(string.IsNullOrEmpty(op) ? DefaultOperation : op.ToUpper())
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

            Expression<Func<TEntity, bool>>? current;
            if (LambdaDictionary.TryGetValue(item.Name, out var value))
            {
                current = value;
            }
            else
            {
                var itemName = item.Name;
                string op = string.Empty;
                var split = itemName.Split(SeparatorCharacter);
                if (split.Length == 2)
                {
                    itemName = split[1];
                    op = split[0];
                }
                else if (split.Length > 3)
                {
                    throw new UnsupportedPropertyNameException(item.Name);
                }
                MemberExpression member = Expression.Property(param, itemName);
                Expression expression = BuildExpression(op, member, constant);
                current = Expression.Lambda<Func<TEntity, bool>>(expression, param);
            }

            final = final == null ? current : final.And(current);
        }
        return final;
    }
}
