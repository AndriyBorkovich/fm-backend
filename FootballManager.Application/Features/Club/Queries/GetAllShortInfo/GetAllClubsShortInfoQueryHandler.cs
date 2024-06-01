using FootballManager.Application.Contracts.Persistence;
using FootballManager.Domain.Enums;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ServiceResult;
using ClubEntity = FootballManager.Domain.Entities.Club;
using PlayerEntity = FootballManager.Domain.Entities.Player;

namespace FootballManager.Application.Features.Club.Queries.GetAllShortInfo;

public record GetAllClubsShortInfoQuery : IRequest<Result<List<GetAllClubsShortInfoResponse>>>;

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
    IClubRepository repository, IMapper mapper)
        : IRequestHandler<GetAllClubsShortInfoQuery, Result<List<GetAllClubsShortInfoResponse>>>
{
    public async Task<Result<List<GetAllClubsShortInfoResponse>>> Handle(GetAllClubsShortInfoQuery request, CancellationToken cancellationToken)
    {
        var clubs = await GetData(cancellationToken);

        var result = mapper.Map<List<GetAllClubsShortInfoResponse>>(clubs);

        return new SuccessResult<List<GetAllClubsShortInfoResponse>>(result);
    }

    private async Task<List<ClubEntity>> GetData(CancellationToken cancellationToken)
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
                        .ToListAsync(cancellationToken);
    }
}
