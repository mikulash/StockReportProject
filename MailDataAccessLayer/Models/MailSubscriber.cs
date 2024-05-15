using System.ComponentModel.DataAnnotations;
using GenericDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace MailDataAccessLayer.Models;

[Index(nameof(Email), IsUnique = true)]
public class MailSubscriber : BaseEntity<Guid>
{
    [MaxLength(200)]
    public required string Email { get; set; }
    
    public virtual IEnumerable<SubscriberPreference>? Preferences { get; set; }
}
