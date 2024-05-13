using MailDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace MailDataAccessLayer.Data;

public class MailDbContext(DbContextOptions<MailDbContext> options) : DbContext(options)
{
    public DbSet<MailSubscriber> MailSubscribers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.SetNull;
        }
        
        base.OnModelCreating(modelBuilder);
    }
}
