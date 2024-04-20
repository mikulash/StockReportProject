using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public class Fund : BaseEntity<long>
{
    [MaxLength(40)]
    public required string FundName { get; set; }
    
    public virtual IEnumerable<IndexRecord>? IndexRecords { get; set; }
}