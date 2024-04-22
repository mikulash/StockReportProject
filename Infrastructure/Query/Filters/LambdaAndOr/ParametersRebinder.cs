using System.Linq.Expressions;

namespace Infrastructure.Query.Filters.LambdaAndOr;

// source: https://learn.microsoft.com/en-us/archive/blogs/meek/linq-to-entities-combining-predicates
public class ParameterRebinder : ExpressionVisitor
{

    private readonly Dictionary<ParameterExpression, ParameterExpression> map;

    public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
    {

        this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();

    }

    public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map, Expression exp)
    {

        return new ParameterRebinder(map).Visit(exp);

    }

    protected override Expression VisitParameter(ParameterExpression p)
    {
        if (map.TryGetValue(p, out var replacement))
        {

            p = replacement;

        }

        return base.VisitParameter(p);

    }

}