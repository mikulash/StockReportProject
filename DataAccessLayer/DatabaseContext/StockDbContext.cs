using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.DatabaseContext;

public class StockDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.SetNull;
        }
        
        
        base.OnModelCreating(modelBuilder);
    }

}