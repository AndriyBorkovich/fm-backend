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
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Club>>();
            mockSet.As<IQueryable<Club>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Player>(mockClubs.Provider));
            mockSet.As<IQueryable<Club>>().Setup(m => m.Expression).Returns(mockClubs.Expression);
            mockSet.As<IQueryable<Club>>().Setup(m => m.ElementType).Returns(mockClubs.ElementType);
            mockSet.As<IQueryable<Club>>().Setup(m => m.GetEnumerator()).Returns(mockClubs.GetEnumerator());
            mockSet.As<IAsyncEnumerable<Club>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<Club>(mockClubs.GetEnumerator()));

            var mockRepo = new Mock<IClubRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(mockSet.Object);
            mockRepo.Setup(r => r.InsertAsync(It.IsAny<Club>()))
                .Returns((Club club) =>
                {
                    var clubsList = mockClubs.ToList();
                    clubsList.Add(club);
                    mockClubs = clubsList.AsQueryable();
                    return Task.CompletedTask;
                });

            return mockRepo;
        }
    }
}
