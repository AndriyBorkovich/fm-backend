using FluentValidation;
using FootballManager.Application.Contracts.Logging;
using FootballManager.Application.Contracts.Persistence;
using MapsterMapper;
using MediatR;
using ServiceResult;

namespace FootballManager.Application.Features.Player.Commands.Update;

public class UpdatePlayerCommandHandler : IRequestHandler<UpdatePlayerCommand, Result<Unit>>
{
    private readonly IMapper _mapper;
    private readonly IPlayerRepository _playerRepository;
    private readonly IAppLogger<UpdatePlayerCommandHandler> _logger;
    private readonly IValidator<UpdatePlayerCommand> _validator;

    public UpdatePlayerCommandHandler(
        IMapper mapper,
        IPlayerRepository playerRepository,
        IAppLogger<UpdatePlayerCommandHandler> logger,
        IValidator<UpdatePlayerCommand> validator)
    {
        _mapper = mapper;
        _playerRepository = playerRepository;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Result<Unit>> Handle(UpdatePlayerCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation errors", validationResult.ToString());
            return new InvalidResult<Unit>(validationResult.ToString());
        }

        var playerToUpdate = await _playerRepository.GetByIdAsync(request.Id)!;
        _mapper.Map(request, playerToUpdate);

        await _playerRepository.UpdateAsync(playerToUpdate);

        return new SuccessResult<Unit>(Unit.Value);
    }
}
