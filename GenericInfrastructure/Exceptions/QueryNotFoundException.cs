namespace GenericInfrastructure.Exceptions;

public class QueryNotFoundException(Type type) : Exception($"Query with entity type <<{type.Name}>>, was not found.")
{
    
}