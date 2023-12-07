using System;
using System.Threading.Tasks;

using System.Threading;
// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================


namespace ChatBox;
public static class AsyncHelper
{
    private static readonly TaskFactory TaskFactory = new
        TaskFactory(CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

    public static TResult Sync<TResult>(Func<Task<TResult>> func)
    {
        return TaskFactory
            .StartNew(func).Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    public static void Sync(Func<Task> func)
    {
        TaskFactory
            .StartNew(func).Unwrap()
            .GetAwaiter()
            .GetResult();
    }
}