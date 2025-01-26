using MediatR;
using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.AppServices.Abstractions;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, BusinessResult>
    where TCommand : ICommand;

public interface ICommandHandler<in TCommand, TResponse>
    : IRequestHandler<TCommand, BusinessResult<TResponse>>
    where TCommand : ICommand<TResponse>;
