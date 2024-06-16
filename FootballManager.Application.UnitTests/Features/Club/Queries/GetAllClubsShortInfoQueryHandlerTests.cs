using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Features.Club.Queries.GetAllShortInfo;
using FootballManager.Application.UnitTests.Mocks;
using FootballManager.Application.Utilities;
using MapsterMapper;
using Moq;
using Shouldly;

namespace FootballManager.Application.UnitTests.Features.Club.Queries
{
    public class GetAllClubsShortInfoQueryHandlerTests
    {
        private readonly Mock<IClubRepository> _mockRepo;
        private readonly IMapper _mapper;

        public GetAllClubsShortInfoQueryHandlerTests()
        {
            _mockRepo = MockClubRepository.GetRepositrory();
            _mapper = MapsterConfiguration.GetMapper();
        }

        //[Fact]
        public async Task GetAllShortInfo_Success()
        {
            var handler = new GetAllClubsShortInfoQueryHandler(_mockRepo.Object, _mapper);

            var result = await handler.Handle(new GetAllClubsShortInfoQuery(new Pagination()), CancellationToken.None);

            result.Errors.ShouldBeEmpty();
            result.Data.ShouldNotBeNull();
        }
    }
}
