using System.Linq.Expressions;
using GenericInfrastructure.Query.Filters.ExpressionStrategy.ExpressionOperation;

namespace GenericInfrastructure.Query.Filters;

public class PropertyFilterParameters
{
    private const char SeparatorCharacter = '_';
    private const Operation DefaultOperation = Operation.EQ;

    private bool _hasOperation = false;
    
    public Operation Operation { get; set; }
    public string[] Members { get; set; } = [];

    public void ParsePropertyName(string name)
    {
        var split = name.Split(SeparatorCharacter);

        Operation = DefaultOperation;
        if (Enum.TryParse(split[0], out Operation op))
        {
            Operation = op;
            _hasOperation = true;
        }

        Members = split.Skip(_hasOperation ? 1 : 0).ToArray();
    }

    public MemberExpression TryCreateMember(ParameterExpression param)
    {
        MemberExpression result = Expression.Property(param, Members[0]);

        for (int index = 1; index < Members.Length; index++)
        {
            result = Expression.Property(result, Members[index]);
        }

        return result;
    }
}
