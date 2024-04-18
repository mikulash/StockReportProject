

using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public abstract class BaseEntity<TKey>
{
    [Key]
    public TKey Id { get; set; }
}