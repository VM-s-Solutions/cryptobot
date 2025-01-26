using VM.CryptoBot.Infra.Validations.Common;

namespace VM.CryptoBot.Infra.Validations.Interfaces;

public interface IValidationResult
{
    static readonly Error ValidationError = new Error("ValidationError", "A validation problem occurred.");

    Error[] Errors { get; }
}
