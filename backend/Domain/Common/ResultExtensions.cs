using MonadicSharp;
using System;

namespace Domain.Common;

public static class ResultExtensions
{
    public static Result<B> SelectMany<A, B>(
        this Result<A> result,
        Func<A, Result<B>> f)
        => result.Bind(f);

    public static Result<C> SelectMany<A, B, C>(
        this Result<A> result,
        Func<A, Result<B>> f,
        Func<A, B, C> project)
        => result.Bind(a => f(a).Map(b => project(a, b)));

    public static Result<B> Select<A, B>(
        this Result<A> result,
        Func<A, B> f)
        => result.Map(f);
}