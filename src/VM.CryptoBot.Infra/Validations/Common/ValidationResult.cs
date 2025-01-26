using VM.CryptoBot.Infra.Validations.Interfaces;

namespace VM.CryptoBot.Infra.Validations.Common;

public sealed class ValidationResult : BusinessResult, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResult WithErrors(Error[] errors) => new(errors);
}
