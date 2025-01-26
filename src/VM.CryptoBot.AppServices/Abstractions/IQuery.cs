using MediatR;
using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.AppServices.Abstractions;

public interface IQuery<TResponse> : IRequest<BusinessResult<TResponse>>;