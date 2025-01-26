using MediatR;
using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.AppServices.Abstractions;

public interface ICommand : IRequest<BusinessResult>;

public interface ICommand<TResponse> : IRequest<BusinessResult<TResponse>>;