using DataAccessLayer.Models;
using Infrastructure.Repository;

namespace BusinessLayer.Facades;

public static class UpdateMappingUtilities
{
    public static void SelfUpdate<TKey>(this BaseEntity<TKey> entity, BaseEntity<TKey> other)
    {
        foreach (var property in 
                 entity.GetType().GetProperties().Where(prop => !prop.Name.Equals(RepositoryConstants.KeyName)))
        {
            property.SetValue(entity, other.GetType().GetProperty(property.Name)?.GetValue(other));
        }
    }
}