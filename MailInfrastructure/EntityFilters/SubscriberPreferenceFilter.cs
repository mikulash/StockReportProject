using GenericInfrastructure.Query.Filters;
using MailDataAccessLayer.Models;

namespace MailInfrastructure.EntityFilters;

public class SubscriberPreferenceFilter: FilterBase<SubscriberPreference>
{
    public string? EQ_FundName { get; set; }
}