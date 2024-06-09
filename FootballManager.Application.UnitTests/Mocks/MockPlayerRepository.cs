using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FootballManager.Application.UnitTests.Mocks;

public class MockPlayerRepository
{
    public static Mock<IPlayerRepository> GetRepository(List<Player>? players = null)
    {
        var mockPlayers = players?.AsQueryable() ?? (new List<Player>
        {
            new()
            {
                Id = 1,
                Name = "Player 1",
                Nationality = "Country 1",
                BirthDay = new DateTime(1999, 2, 1),
                Position = PlayerPosition.Forward,
                ClubId = 1
            },
            new()
            {
                Id = 2,
                CreatedDate = DateTime.Now.AddHours(-2),
                Name = "Player 2",
                Nationality = "Country 2",
                BirthDay = new DateTime(1989, 12, 15),
                Position = PlayerPosition.Midfielder,
                ClubId = 2
            },
            new()
            {
                Id = 3,
                CreatedDate = DateTime.Now.AddHours(-1),
                Name = "Player 2", Nationality = "Country 2",
                BirthDay = new DateTime(1999, 3, 12),
                Position = PlayerPosition.Defender,
                ClubId = 2
            }
        }).AsQueryable();

        var mockSet = new Mock<DbSet<Player>>();
        mockSet.As<IQueryable<Player>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Player>(mockPlayers.Provider));
        mockSet.As<IQueryable<Player>>().Setup(m => m.Expression).Returns(mockPlayers.Expression);
        mockSet.As<IQueryable<Player>>().Setup(m => m.ElementType).Returns(mockPlayers.ElementType);
        mockSet.As<IQueryable<Player>>().Setup(m => m.GetEnumerator()).Returns(mockPlayers.GetEnumerator());
        mockSet.As<IAsyncEnumerable<Player>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<Player>(mockPlayers.GetEnumerator()));

        var mockRepo = new Mock<IPlayerRepository>();
        mockRepo.Setup(r => r.GetAll()).Returns(mockSet.Object);
        mockRepo.Setup(r => r.InsertAsync(It.IsAny<Player>())).Returns((Player player) =>
        {
            var playersList = mockPlayers.ToList();
            playersList.Add(player);
            mockPlayers = playersList.AsQueryable();
            return Task.CompletedTask;
        });

        return mockRepo;
    }
}
