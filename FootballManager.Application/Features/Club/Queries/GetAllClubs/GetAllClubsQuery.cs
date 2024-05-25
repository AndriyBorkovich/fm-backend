using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Queries.GetAllClubs;

public record GetAllClubsQuery : IRequest<Result<List<ClubShortInfoDTO>>>;
