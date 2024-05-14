using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GenericDataAccessLayer.Models;
using MailDataAccessLayer.Enums;

namespace MailDataAccessLayer.Models;

public class SubscriberPreference : BaseEntity<long>
{
    [MaxLength(40)]
    public required string FundName { get; set; }
    public required OutputType OutputType { get; set; }
    
    public Guid MailSubscriberId { get; set; }
    [ForeignKey(nameof(MailSubscriberId))]
    public virtual MailSubscriber? MailSubscriber { get; set; }
}
