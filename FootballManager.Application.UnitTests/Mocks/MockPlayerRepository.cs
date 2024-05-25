using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FootballManager.Application.UnitTests.Mocks;

public class MockPlayerRepository
{
    public static Mock<IPlayerRepository> GetPlayerRepository()
    {
        var mockPlayers = new List<Player>
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
        };

        var mockRepo = new Mock<IPlayerRepository>();
        mockRepo.Setup(r => r.GetPlayersShortInfo()).ReturnsAsync(mockPlayers);
        mockRepo.Setup(r => r.InsertAsync(It.IsAny<Player>()))
            .Returns((Player player) =>
            {
                mockPlayers.Add(player);
                return Task.CompletedTask;
            });

        return mockRepo;
    }
}
