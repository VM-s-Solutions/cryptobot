using MediatR;
using VM.CryptoBot.Domain.Store;

namespace VM.CryptoBot.AppServices.Behaviors;

public class UnitOfWorkPipelineBehavior<TRequest, TResponse>(IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (IsNotCommand(request))
        {
            return await next();
        }

        var response = await next();
        await unitOfWork.CommitAsync(cancellationToken);

        return response;
    }

    private static bool IsNotCommand(TRequest request)
    {
        return !request.GetType().Name.EndsWith("Command");
    }
}
