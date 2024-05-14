using System.ComponentModel.DataAnnotations;
using GenericDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models;

[Index(nameof(CUSIP), IsUnique = true)]
public class Company : BaseEntity<long>
{
    [MaxLength(50)]
    public required string CompanyName { get; set; }
    [MaxLength(20)]
    public required string Ticker { get; set; }
    [MaxLength(9)]
    public required string CUSIP { get; set; }
    
    public virtual IEnumerable<IndexRecord>? IndexRecords { get; set; }
}