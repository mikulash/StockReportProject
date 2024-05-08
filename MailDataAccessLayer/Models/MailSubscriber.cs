using System.ComponentModel.DataAnnotations;
using GenericDataAccessLayer.Models;

namespace MailDataAccessLayer.Models;

public class MailSubscriber : BaseEntity<long>
{
    [MaxLength(200)]
    public required string Email { get; set; }
}
