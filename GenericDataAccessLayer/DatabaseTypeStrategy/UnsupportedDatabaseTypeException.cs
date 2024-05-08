using GenericDataAccessLayer.DatabaseTypeStrategy.DatabaseStrategies;

namespace GenericDataAccessLayer.DatabaseTypeStrategy;

public class UnsupportedDatabaseTypeException(DatabaseType dbType)
    : Exception($"Database Type <<{dbType}>> is not supported and could not be initialized!");
