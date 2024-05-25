using FootballManager.Application.Features.Shared.DTOs;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Match.Commands.SimulateMatch;

public record SimulateMatchCommand(int HomeTeamId, int AwayTeamId) : IRequest<Result<MatchResultDTO>>;
