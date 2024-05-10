using GenericInfrastructure.Query.Filters;
using MailDataAccessLayer.Models;

namespace MailInfrastructure.EntityFilters;

public class MailSubscriberFilter : FilterBase<MailSubscriber>
{
    public string? CONTAINS_Email { get; set; }
}

public class MailSubscriberExactFilter : FilterBase<MailSubscriber>
{
    public string? EQ_Email { get; set; }
}
