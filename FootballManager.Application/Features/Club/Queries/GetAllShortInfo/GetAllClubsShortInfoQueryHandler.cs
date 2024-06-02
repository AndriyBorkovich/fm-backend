using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using FootballManager.Application.Utilities;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;
using ClubEntity = FootballManager.Domain.Entities.Club;
using PlayerEntity = FootballManager.Domain.Entities.Player;

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
        var clubsData = await GetData(request.Pagination, cancellationToken);

        var result = mapper.Map<List<GetAllClubsShortInfoResponse>>(clubsData.List);

        return new SuccessResult<ListResponse<GetAllClubsShortInfoResponse>>(
            new ListResponse<GetAllClubsShortInfoResponse>(result, clubsData.Count));
    }

    private async Task<(List<ClubEntity> List, int Count)> GetData(Pagination pagination, CancellationToken cancellationToken)
    {
        return await repository
                        .GetAll()
                        .AsNoTracking()
                        .Select(c => new ClubEntity
                        {
                            Id = c.Id,
                            Name = c.Name,
                            StadiumName = c.StadiumName,
                            Type = c.Type,
                            Players = c.Players.Select(p => new PlayerEntity
                            {
                                Id = p.Id
                            }).ToList()
                        })
                        .Page(pagination);
    }
}
