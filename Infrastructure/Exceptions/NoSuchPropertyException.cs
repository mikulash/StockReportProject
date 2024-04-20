
namespace Infrastructure.Exceptions;

public class NoSuchPropertyException(string prop) : Exception($"Property with Name <<{prop}>> does not exist!");
