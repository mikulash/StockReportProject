using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models;

public class Company : BaseEntity<long>
{
    [MaxLength(50)]
    public required string CompanyName { get; set; }
    [MaxLength(20)]
    public required string Ticker { get; set; }
    [MaxLength(9)]
    public required string Cusip { get; set; }
    
    public IEnumerable<IndexRecord>? IndexRecords { get; set; }
}