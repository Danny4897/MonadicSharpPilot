using FunctionalSharp;
using System;
using System.Threading.Tasks;

namespace Domain.Common;

public static class TaskExtensions
{
    public static async Task<B> Map<A, B>(this Task<A> task, Func<A, B> f)
    {
        var a = await task;
        return f(a);
    }

    public static async Task<B> Bind<A, B>(this Task<A> task, Func<A, Task<B>> f)
    {
        var a = await task;
        return await f(a);
    }

    public static async Task<Result<B>> Map<A, B>(this Task<A> task, Func<A, Result<B>> f)
    {
        var a = await task;
        return f(a);
    }

    public static async Task<Result<B>> Bind<A, B>(this Task<A> task, Func<A, Task<Result<B>>> f)
    {
        var a = await task;
        return await f(a);
    }
}