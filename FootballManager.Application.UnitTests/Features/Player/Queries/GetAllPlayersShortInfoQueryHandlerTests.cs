using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Features.Player.Queries.GetAllShortInfo;
using FootballManager.Application.UnitTests.Mocks;
using FootballManager.Application.Utilities;
using MapsterMapper;
using Moq;
using Shouldly;

namespace FootballManager.Application.UnitTests.Features.Player.Queries;

public class GetAllPlayersShortInfoQueryHandlerTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IClubRepository> _mockClubRepo;
    private readonly Mock<IAppLogger<GetAllPlayersShortInfoQueryHandler>> _logger;

    public GetAllPlayersShortInfoQueryHandlerTests()
    {
        _mapper = MapsterConfiguration.GetMapper();
        _logger = new Mock<IAppLogger<GetAllPlayersShortInfoQueryHandler>>();
        _mockClubRepo = MockClubRepository.GetRepositrory();
    }

    [Fact]
    public async Task Handle_Success_ReturnsDefaultCount()
    {
        var mockRepo = MockPlayerRepository.GetRepository();

        var handler = new GetAllPlayersShortInfoQueryHandler(_mapper, _logger.Object, _mockClubRepo.Object, mockRepo.Object);
        var result = await handler.Handle(new GetAllPlayersShortInfoQuery(null, new Pagination()), CancellationToken.None);

        result.Errors.ShouldBeEmpty();

        var resultList = result.Data;
        resultList.ShouldBeOfType<ListResponse<GetAllPlayersShortInfoResponse>>();
        resultList.Total.ShouldBe(3);
    }

    [Fact]
    public async Task Handle_Success_ReturnsNoElements()
    {
        var mockRepo = MockPlayerRepository.GetRepository([]);

        var handler = new GetAllPlayersShortInfoQueryHandler(_mapper, _logger.Object, _mockClubRepo.Object, mockRepo.Object);
        var result = await handler.Handle(new GetAllPlayersShortInfoQuery(null, new Pagination()), CancellationToken.None);

        result.Errors.ShouldBeEmpty();

        var resultList = result.Data;
        resultList.ShouldBeOfType<ListResponse<GetAllPlayersShortInfoResponse>>();
        resultList.Total.ShouldBe(0);
    }
}
