using FunctionalSharp;

namespace Domain.Common;

public static class NullableExtensions
{
    public static Option<T> ToOption<T>(this T? value) where T : class
        => value != null ? Option<T>.Some(value) : Option<T>.None;

    public static Option<T> ToOption<T>(this T? value) where T : struct
        => value.HasValue ? Option<T>.Some(value.Value) : Option<T>.None;
}