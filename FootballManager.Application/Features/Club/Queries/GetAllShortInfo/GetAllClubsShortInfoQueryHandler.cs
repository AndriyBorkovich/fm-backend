using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using FootballManager.Application.Utilities;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetAllShortInfo;

public record GetAllClubsShortInfoQuery(Pagination Pagination) : IRequest<Result<ListResponse<GetAllClubsShortInfoResponse>>>;

public record GetAllClubsShortInfoResponse
(
    int Id,
    string Name,
    string StadiumName,
    string HeadCoachName,
    ClubType Type,
    int PlayersCount
);

public class GetAllClubsShortInfoQueryHandler(
    IClubRepository repository,
    IMapper mapper)
        : IRequestHandler<GetAllClubsShortInfoQuery, Result<ListResponse<GetAllClubsShortInfoResponse>>>
{
    public async Task<Result<ListResponse<GetAllClubsShortInfoResponse>>> Handle(GetAllClubsShortInfoQuery request, CancellationToken cancellationToken)
    {
        var (items, total) = await repository
                                .GetAllShortInfo()
                                .Page(request.Pagination, cancellationToken);

        var result = mapper.Map<List<GetAllClubsShortInfoResponse>>(items);

        return new SuccessResult<ListResponse<GetAllClubsShortInfoResponse>>(new(result, total));
    }
}
