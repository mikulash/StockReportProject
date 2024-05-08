namespace GenericInfrastructure.Exceptions;

public class RepositoryNotFoundException(Type type) : Exception($"Entity of type <<{type.Name}>>, was not found.")
{
    public Type EntityType { get; } = type;
}
