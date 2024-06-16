using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Entities;
using FootballManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FootballManager.Application.UnitTests.Mocks
{
    public class MockClubRepository
    {
        public static Mock<IClubRepository> GetRepositrory()
        {
            var mockClubs = new List<Club>
            {
                new() {
                    Id = 1,
                    Name = "Club A",
                    StadiumName = "Stadium A",
                    Type = ClubType.Standard,
                    Players =
                    [
                        new() { Id = 1 },
                        new() { Id = 2 }
                    ]
                },
                new() {
                    Id = 2,
                    Name = "Club B",
                    StadiumName = "Stadium B",
                    Type = ClubType.Standard,
                    Players =
                    [
                        new() { Id = 3 }
                    ]
                }
            };

            var mockRepo = new Mock<IClubRepository>();
            mockRepo.Setup(r => GetData(r)).ReturnsAsync(mockClubs);
            mockRepo.Setup(r => r.InsertAsync(It.IsAny<Club>()))
                .Returns((Club club) =>
                {
                    mockClubs.Add(club);
                    return Task.CompletedTask;
                });

            return mockRepo;
        }

        private static async Task<List<Club>> GetData(IClubRepository repository)
        {
            return await repository.GetAll().ToListAsync();
        }
    }
}
