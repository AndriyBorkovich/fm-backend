using FootballManager.Domain.Entities;
using FootballManager.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FootballManager.Persistence.DatabaseContext;

public class FootballManagerContext : IdentityDbContext<AppUser, IdentityRole<string>, string>
{
    public FootballManagerContext(DbContextOptions<FootballManagerContext> options) : base(options)
    {

    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Club> Clubs { get; set; }
    public DbSet<GoalAction> GoalActions { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<Coach> Coaches { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Championship> Championships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(FootballManagerContext).Assembly);
        modelBuilder.ApplyConfiguration(new RoleConfiguration());

        modelBuilder.Entity<Match>()
            .HasOne(m => m.HomeTeam)
            .WithMany(c => c.HomeMatches)
            .HasForeignKey(m => m.HomeTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.AwayTeam)
            .WithMany(c => c.AwayMatches)
            .HasForeignKey(m => m.AwayTeamId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GoalAction>()
            .HasOne(g => g.Scorer)
            .WithMany(p => p.ScoredGoals)
            .HasForeignKey(g => g.ScorerId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<GoalAction>()
            .HasOne(g => g.Assistant)
            .WithMany(p => p.AssistedGoals)
            .HasForeignKey(g => g.AssistantId)
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);
    }
}
