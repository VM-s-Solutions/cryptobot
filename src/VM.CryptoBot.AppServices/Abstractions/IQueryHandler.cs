using MediatR;
using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.AppServices.Abstractions;

public interface IQueryHandler<in TQuery, TResponse>
    : IRequestHandler<TQuery, BusinessResult<TResponse>>
    where TQuery : IQuery<TResponse>;