using System.ComponentModel.DataAnnotations;
using GenericDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Models;

[Index(nameof(FundName), IsUnique = true)]
public class Fund : BaseEntity<long>
{
    [MaxLength(40)]
    public required string FundName { get; set; }
    
    public virtual IEnumerable<IndexRecord>? IndexRecords { get; set; }
}