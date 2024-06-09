using FootballManager.Application.UnitTests.Mocks;
using FootballManager.Domain.Enums;
using MapsterMapper;
using FootballManager.Application.Features.Player.Queries.GetPlayerWithStats;
using Shouldly;
using ServiceResult;
using Moq;
using System.Linq.Expressions;
using PlayerEntity = FootballManager.Domain.Entities.Player;
using ClubEntity = FootballManager.Domain.Entities.Club;

namespace FootballManager.Application.UnitTests.Features.Player.Queries
{
    public class GetPlayerWithStatsQueryHandlerTests
    {
        private readonly IMapper _mapper;
        public GetPlayerWithStatsQueryHandlerTests()
        {
            _mapper = MapsterConfiguration.GetMapper();
        }

        [Fact]
        public async Task Handle_PlayerExists_ReturnCorrectStats()
        {
            var playerId = 1;
            var player = new PlayerEntity
            {
                Id = playerId,
                Name = "Player 1",
                Position = PlayerPosition.Forward,
                BirthDay = new DateTime(1999, 2, 1),
                CurrentClub = new ClubEntity { Name = "Club 1" },
                Matches = [new(), new()],
                ScoredGoals = [new() { IsOwnGoal = false }, new() { IsOwnGoal = false }],
                AssistedGoals = [new(), new()],
                Cards =
                [
                    new() { Type = CardType.Yellow },
                    new() { Type = CardType.Red },
                    new() {Type = CardType.Yellow },
                ]
            };

            var mockRepo = MockPlayerRepository.GetRepository([player]);

            mockRepo.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<PlayerEntity, bool>>>()))
            .ReturnsAsync(true);

            mockRepo.Setup(repo => repo.GetPlayerWithStats(playerId)).ReturnsAsync(player);

            var handler = new GetPlayerWithStatsQueryHandler(_mapper, mockRepo.Object);

            var result = await handler.Handle(new GetPlayerWithStatsQuery(playerId), CancellationToken.None);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<SuccessResult<GetPlayerWithStatsResponse>>();

            var response = result.Data;
            response.ShouldNotBeNull();
            response.Name.ShouldBe(player.Name);
            response.ClubName.ShouldBe(player.CurrentClub.Name);
            response.Position.ShouldBe("Forward");
            response.Age.ShouldBe(player.CalculateAge());
            response.MatchesPlayed.ShouldBe(player.Matches.Count);
            response.GoalsScored.ShouldBe(player.ScoredGoals.Count(g => !g.IsOwnGoal));
            response.Assists.ShouldBe(player.AssistedGoals.Count);
            response.RedCards.ShouldBe(player.Cards.Count(c => c.Type == CardType.Red));
            response.YellowCards.ShouldBe(player.Cards.Count(c => c.Type == CardType.Yellow));
        }

        [Fact]
        public async Task Handle_PlayerNotFound_ShouldReturnNotFoundResult()
        {
            var playerId = 99;

            var mockRepo = MockPlayerRepository.GetRepository();

            mockRepo.Setup(repo => repo.AnyAsync(It.IsAny<Expression<Func<PlayerEntity, bool>>>()))
                .ReturnsAsync(false);

            var query = new GetPlayerWithStatsQuery(playerId);

            var handler = new GetPlayerWithStatsQueryHandler(_mapper, mockRepo.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            result.ShouldBeOfType<NotFoundResult<GetPlayerWithStatsResponse>>();
        }
    }
}
