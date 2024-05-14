using MailDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace MailDataAccessLayer.Data;

public class MailDbContext(DbContextOptions<MailDbContext> options) : DbContext(options)
{
    public DbSet<MailSubscriber> MailSubscribers { get; set; }
    public DbSet<SubscriberPreference> SubscriberPreferences { get; set; }

    private static void SetUpDatabaseRelations(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MailSubscriber>()
            .HasMany<SubscriberPreference>(sub => sub.Preferences)
            .WithOne(pref => pref.MailSubscriber)
            .HasForeignKey(pref => pref.MailSubscriberId)
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
