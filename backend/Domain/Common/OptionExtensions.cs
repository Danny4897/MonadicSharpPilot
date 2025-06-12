using FunctionalSharp;

namespace Domain.Common;

public static class OptionExtensions
{
    public static Result<T> ToResult<T>(this Option<T> option, string errorMessage)
        => option.Match(
            value => Result<T>.Success(value),
            () => Result<T>.Failure(errorMessage));
}