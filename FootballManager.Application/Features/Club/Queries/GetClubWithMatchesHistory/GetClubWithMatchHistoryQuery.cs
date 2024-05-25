using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetClubWithMatchesHistory;

public record GetClubWithMatchHistoryQuery(int Id) : IRequest<Result<ClubWithMatchHistoryDTO>>;
