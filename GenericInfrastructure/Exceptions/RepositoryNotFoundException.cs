namespace GenericInfrastructure.Exceptions;

public class RepositoryNotFoundException(Type type) : Exception($"Repository with entity type <<{type.Name}>>, was not found.")
{
    public Type EntityType { get; } = type;
}
