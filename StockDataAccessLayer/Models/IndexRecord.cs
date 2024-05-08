using System.ComponentModel.DataAnnotations.Schema;
using GenericDataAccessLayer.Models;

namespace DataAccessLayer.Models;

public class IndexRecord : BaseEntity<long>
{
    public long FundId { get; set; }
    [ForeignKey(nameof(FundId))]
    public virtual Fund? Fund { get; set; }
    
    public long CompanyId { get; set; }
    [ForeignKey(nameof(CompanyId))]
    public virtual Company? Company { get; set; }
    
    public DateOnly IssueDate { get; set; }
    public long Shares { get; set; }
    public double MarketValue { get; set; }
    public double Weight { get; set; }
}