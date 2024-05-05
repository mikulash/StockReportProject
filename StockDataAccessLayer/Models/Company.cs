﻿using System.ComponentModel.DataAnnotations;
using GenericDataAccessLayer.Models;

namespace DataAccessLayer.Models;

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