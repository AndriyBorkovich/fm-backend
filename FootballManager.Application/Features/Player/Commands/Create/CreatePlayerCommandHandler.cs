using FluentValidation;
using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using FootballManager.Application.Extensions;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Commands.Create;

public class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Result<int>>
{
    private readonly IMapper _mapper;
    private readonly IAppLogger<CreatePlayerCommandHandler> _logger;
    private readonly IPlayerRepository _playerRepository;
    private readonly IValidator<CreatePlayerCommand> _validator;

    public CreatePlayerCommandHandler(
        IMapper mapper,
        IAppLogger<CreatePlayerCommandHandler> logger,
        IPlayerRepository playerRepository,
        IValidator<CreatePlayerCommand> validator)
    {
        _mapper = mapper;
        _logger = logger;
        _playerRepository = playerRepository;
        _validator = validator;
    }

    public async Task<Result<int>> Handle(CreatePlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors", validationResult.Errors);
            return new InvalidResult<int>(validationResult.Errors.ToResponse());
        }

        var playerToCreate = _mapper.Map<Domain.Entities.Player>(request);

        await _playerRepository.InsertAsync(playerToCreate);

        return new SuccessResult<int>(playerToCreate.Id);
    }
}
