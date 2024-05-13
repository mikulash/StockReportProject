using System.ComponentModel.DataAnnotations;

namespace GenericDataAccessLayer.Models;

public abstract class BaseEntity<TKey>
{
    [Key]
    public TKey Id { get; set; }
}