using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Club.Commands.Create;

public class CreateClubCommandHandler : IRequestHandler<CreateClubCommand, Result<int>>
{
    private readonly IClubRepository _clubRepository;
    private readonly IMapper _mapper;

    public CreateClubCommandHandler(IClubRepository clubRepository, IMapper mapper)
    {
        _clubRepository = clubRepository;
        _mapper = mapper;
    }

    public async Task<Result<int>> Handle(CreateClubCommand request, CancellationToken cancellationToken)
    {
        var club = _mapper.Map<Domain.Entities.Club>(request);
        await _clubRepository.InsertAsync(club);

        return new SuccessResult<int>(club.Id);
    }
}
