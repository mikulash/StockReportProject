using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Data;

public class StockDbContext(DbContextOptions<StockDbContext> options) : DbContext(options)
{
    public DbSet<Fund> Funds { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<IndexRecord> Holdings { get; set; }


    private static void SetUpDatabaseRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fund>()
            .HasMany(fund => fund.IndexRecords)
            .WithOne(indexRecord => indexRecord.Fund)
            .HasForeignKey(indexRecord => indexRecord.FundId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Company>()
            .HasMany(comp => comp.IndexRecords)
            .WithOne(indexRecord => indexRecord.Company)
            .HasForeignKey(indexRecord => indexRecord.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.SetNull;
        }
        
        SetUpDatabaseRelations(modelBuilder);
        
        base.OnModelCreating(modelBuilder);
    }

}