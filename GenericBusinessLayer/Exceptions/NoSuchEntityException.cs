namespace GenericBusinessLayer.Exceptions;

public class NoSuchEntityException<TKey> : Exception
{
    public NoSuchEntityException(Type type, TKey id) 
        : base($"Entity {type.Name} with given id ({id}) does not exist!")
    {
    }

    public NoSuchEntityException(Type type, IEnumerable<TKey> ids)
        : base($"Entities {type.Name} with ids ({ids}) do not exist!")
    {
    }

    public NoSuchEntityException(Type type)
        : base($"Entity {type.Name} does not exist!")
    {
    }
}
