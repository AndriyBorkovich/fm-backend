using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Features.Player.Queries.GetAllShortInfo;
using FootballManager.Application.UnitTests.Mocks;
using MapsterMapper;
using Moq;
using Shouldly;

namespace FootballManager.Application.UnitTests.Features.Player.Queries;

public class GetAllPlayersQueryHandlerTests
{
    private readonly Mock<IPlayerRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly Mock<IAppLogger<GetAllPlayersQueryHandler>> _logger;

    public GetAllPlayersQueryHandlerTests()
    {
        _mockRepo = MockPlayerRepository.GetPlayerRepository();
        _mapper = MapsterConfiguration.GetMapper();
        _logger = new Mock<IAppLogger<GetAllPlayersQueryHandler>>();
    }

    [Fact]
    public async Task GetAllPlayers_Success()
    {
        var handler = new GetAllPlayersQueryHandler(_mapper, _logger.Object, _mockRepo.Object);
        var result = await handler.Handle(new GetAllPlayersQuery(), CancellationToken.None);

        var resultList = result.Data;
        resultList.ShouldBeOfType<List<GetAllPlayersShortInfoResponse>>();
        resultList.Count.ShouldBe(3);
    }
}
