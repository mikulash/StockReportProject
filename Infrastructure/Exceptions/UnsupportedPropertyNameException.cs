namespace Infrastructure.Exceptions;

public class UnsupportedPropertyNameException(string name)
    : Exception($"Query Filter property with name {name} is not supported or is not instantiated correctly!");
