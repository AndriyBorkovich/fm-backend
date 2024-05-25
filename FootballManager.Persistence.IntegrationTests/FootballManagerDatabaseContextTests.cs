using FootballManager.Domain.Entities;
using FootballManager.Persistence.DatabaseContext;
using FootballManager.Persistence.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shouldly;

namespace FootballManager.Persistence.IntegrationTests;

public class FootballManagerDatabaseContextTests
{
    private FootballManagerContext _footballManagerContext;

    public FootballManagerDatabaseContextTests()
    {
        var dbOptions = new DbContextOptionsBuilder<FootballManagerContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(b =>
            {
                b.Ignore(InMemoryEventId.ChangesSaved);
                b.Ignore(CoreEventId.ContextInitialized);
            })
            .UseTriggers(triggerConfig =>
            {
                triggerConfig.AddTrigger<BaseEntityUpdatesTrigger>();
            })
            .Options;

        _footballManagerContext = new FootballManagerContext(dbOptions);
    }

    [Fact]
    public async Task SaveChanges_SetCreatedAndUpdatedDate_Successful()
    {
        // Arrange
        var player = new Player()
        {
            Id = 1, BirthDay = new DateTime(2001, 12, 2), Name = "Roberto Bagio", Nationality = "Italy"
        };

        // Act
        await _footballManagerContext.Players.AddAsync(player);
        await _footballManagerContext.SaveChangesAsync();

        // Assert
        player.CreatedDate.Date.ShouldBe(DateTime.UtcNow.Date);
    }
}
