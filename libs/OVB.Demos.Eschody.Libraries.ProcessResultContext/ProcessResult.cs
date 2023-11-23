using OVB.Demos.Eschody.Libraries.ProcessResultContext.Enums;
using OVB.Demos.Eschody.Libraries.ProcessResultContext.Exceptions;

namespace OVB.Demos.Eschody.Libraries.ProcessResultContext;

public readonly struct ProcessResult<TNotification>
{
    public TypeProcessResult TypeProcessResult { get; init; }
    public bool IsSuccess => TypeProcessResult == TypeProcessResult.Success;
    public bool IsError => TypeProcessResult == TypeProcessResult.Error;
    public bool IsPartial => TypeProcessResult == TypeProcessResult.Partial;
    public TNotification[]? Notifications { get; }
    public Exception[]? Exceptions { get; }

    private ProcessResult(
        TypeProcessResult typeProcessResult,
        TNotification[]? notifications,
        Exception[]? exceptions)
    {
        ProcessResultException.ThrowExceptionIfProcessResultTypeIsNotValid(typeProcessResult);

        TypeProcessResult = typeProcessResult;
        Notifications = notifications;
        Exceptions = exceptions;
    }

    #region Builders

    public static ProcessResult<TNotification> BuildProcessResult(
        TypeProcessResult typeProcessResult, TNotification[]? notifications = null, Exception[]? exceptions = null)
        => new ProcessResult<TNotification>(typeProcessResult, notifications, exceptions);
    public static ProcessResult<TNotification> BuildSuccessfullProcessResult(TNotification[]? notifications = null, Exception[]? exceptions = null)
        => BuildProcessResult(TypeProcessResult.Success, notifications, exceptions);
    public static ProcessResult<TNotification> BuildErrorfullProcessResult(TNotification[]? notifications = null, Exception[]? exceptions = null)
        => BuildProcessResult(TypeProcessResult.Error, notifications, exceptions);
    public static ProcessResult<TNotification> BuildPartialProcessResult(TNotification[]? notifications = null, Exception[]? exceptions = null)
        => BuildProcessResult(TypeProcessResult.Partial, notifications, exceptions);
    public static ProcessResult<TNotification> BuildFromAnotherProcessResult(ProcessResult<TNotification> processResult)
        => BuildProcessResult(processResult.TypeProcessResult, processResult.Notifications, processResult.Exceptions);
    public static ProcessResult<TNotification> BuildFromAnotherProcessResult(params ProcessResult<TNotification>[] processResults)
    {

        var totalNotifications = 0;
        var totalExceptions = 0;

        for (int i = 0; i < processResults.Length; i++)
        {
            totalNotifications += processResults[i].Notifications?.Length ?? 0;
            totalExceptions += processResults[i].Notifications?.Length ?? 0;
        }

        TypeProcessResult? newTypeProcessResult = null;

        TNotification[]? newMessageArray = null;
        newMessageArray = new TNotification[totalNotifications];

        Exception[]? newExceptionArray = null;
        newExceptionArray = new Exception[totalExceptions];

        var lastMessageIndex = 0;
        var lastExceptionIndex = 0;

        for (int i = 0; i < processResults.Length; i++)
        {
            var processResult = processResults[i];

            if (newTypeProcessResult is null)
                newTypeProcessResult = processResult.TypeProcessResult;
            else if (newTypeProcessResult == TypeProcessResult.Success && processResult.TypeProcessResult != TypeProcessResult.Success)
                newTypeProcessResult = processResult.TypeProcessResult;

            if (processResult.Notifications is not null)
            {
                Array.Copy(
                    sourceArray: processResult.Notifications,
                    sourceIndex: 0,
                    destinationArray: newMessageArray!,
                    destinationIndex: lastMessageIndex,
                    length: processResult.Notifications.Length
                );

                lastMessageIndex += processResult.Notifications.Length;
            }

            if (processResult.Exceptions is not null)
            {
                Array.Copy(
                    sourceArray: processResult.Exceptions,
                    sourceIndex: 0,
                    destinationArray: newExceptionArray!,
                    destinationIndex: lastExceptionIndex,
                    length: processResult.Exceptions.Length
                );

                lastExceptionIndex += processResult.Exceptions.Length;
            }
        }

        return new ProcessResult<TNotification>(newTypeProcessResult!.Value, newMessageArray, newExceptionArray);
    }

    #region Execution Encapsulated

    public static ProcessResult<TNotification> Execute(Func<ProcessResult<TNotification>> handler)
        => handler();
    public static Task<ProcessResult<TNotification>> Execute(Func<Task<ProcessResult<TNotification>>> handler)
        => handler();
    public static ProcessResult<TNotification> Execute<TInput>(TInput input, Func<TInput, ProcessResult<TNotification>> handler)
        => handler(input);
    public static Task<ProcessResult<TNotification>> Execute<TInput>(TInput input, Func<TInput, Task<ProcessResult<TNotification>>> handler)
        => handler(input);

    #endregion

    #endregion
}

public readonly struct ProcessResult<TNotification, TOutput>
{
    public TypeProcessResult TypeProcessResult { get; init; }
    public bool IsSuccess => TypeProcessResult == TypeProcessResult.Success;
    public bool IsError => TypeProcessResult == TypeProcessResult.Error;
    public bool IsPartial => TypeProcessResult == TypeProcessResult.Partial;
    public bool HasOutput => Output != null;
    public TNotification[]? Notifications { get; }
    public TOutput? Output { get; init; }
    public Exception[]? Exceptions { get; }

    private ProcessResult(
        TypeProcessResult typeProcessResult,
        TOutput? output,
        TNotification[]? notifications,
        Exception[]? exceptions)
    {
        ProcessResultException.ThrowExceptionIfProcessResultTypeIsNotValid(typeProcessResult);

        TypeProcessResult = typeProcessResult;
        Output = output;
        Notifications = notifications;
        Exceptions = exceptions;
    }

    #region Builders

    public static ProcessResult<TNotification, TOutput> BuildProcessResult(
        TypeProcessResult typeProcessResult, TOutput? output, TNotification[]? notifications = null, Exception[]? exceptions = null)
        => new ProcessResult<TNotification, TOutput>(typeProcessResult, output, notifications, exceptions);
    public static ProcessResult<TNotification, TOutput> BuildSuccessfullProcessResult(TOutput? output, TNotification[]? notifications = null, Exception[]? exceptions = null)
        => BuildProcessResult(TypeProcessResult.Success, output, notifications, exceptions);
    public static ProcessResult<TNotification, TOutput> BuildErrorfullProcessResult(TOutput? output, TNotification[]? notifications = null, Exception[]? exceptions = null)
        => BuildProcessResult(TypeProcessResult.Error, output, notifications, exceptions);
    public static ProcessResult<TNotification, TOutput> BuildPartialProcessResult(TOutput? output, TNotification[]? notifications = null, Exception[]? exceptions = null)
        => BuildProcessResult(TypeProcessResult.Partial, output, notifications, exceptions);
    public static ProcessResult<TNotification, TOutput> BuildFromAnotherProcessResult(ProcessResult<TNotification, TOutput> processResult)
        => BuildProcessResult(processResult.TypeProcessResult, processResult.Output, processResult.Notifications, processResult.Exceptions);
    public static ProcessResult<TNotification, TOutput> BuildFromAnotherProcessResult(TOutput? output,
        params ProcessResult<TNotification>[] processResults)
    {
        var totalNotifications = 0;
        var totalExceptions = 0;

        for (int i = 0; i < processResults.Length; i++)
        {
            totalNotifications += processResults[i].Notifications?.Length ?? 0;
            totalExceptions += processResults[i].Notifications?.Length ?? 0;
        }

        TypeProcessResult? newTypeProcessResult = null;

        TNotification[]? newMessageArray = null;
        newMessageArray = new TNotification[totalNotifications];

        Exception[]? newExceptionArray = null;
        newExceptionArray = new Exception[totalExceptions];

        var lastMessageIndex = 0;
        var lastExceptionIndex = 0;

        for (int i = 0; i < processResults.Length; i++)
        {
            var processResult = processResults[i];

            if (newTypeProcessResult is null)
                newTypeProcessResult = processResult.TypeProcessResult;
            else if (newTypeProcessResult == TypeProcessResult.Success && processResult.TypeProcessResult != TypeProcessResult.Success)
                newTypeProcessResult = processResult.TypeProcessResult;

            if (processResult.Notifications is not null)
            {
                Array.Copy(
                    sourceArray: processResult.Notifications,
                    sourceIndex: 0,
                    destinationArray: newMessageArray!,
                    destinationIndex: lastMessageIndex,
                    length: processResult.Notifications.Length
                );

                lastMessageIndex += processResult.Notifications.Length;
            }

            if (processResult.Exceptions is not null)
            {
                Array.Copy(
                    sourceArray: processResult.Exceptions,
                    sourceIndex: 0,
                    destinationArray: newExceptionArray!,
                    destinationIndex: lastExceptionIndex,
                    length: processResult.Exceptions.Length
                );

                lastExceptionIndex += processResult.Exceptions.Length;
            }
        }

        return new ProcessResult<TNotification, TOutput>(newTypeProcessResult!.Value, output, newMessageArray, newExceptionArray);
    }
    public static ProcessResult<TNotification, TOutput> BuildFromAnotherProcessResult(ProcessResult<TNotification, TOutput> outputProcessResult,
        params ProcessResult<TNotification>[] processResults)
    {
        var totalNotifications = 0;
        var totalExceptions = 0;

        for (int i = 0; i < processResults.Length; i++)
        {
            totalNotifications += processResults[i].Notifications?.Length ?? 0;
            totalExceptions += processResults[i].Notifications?.Length ?? 0;
        }

        TypeProcessResult? newTypeProcessResult = null;

        TNotification[]? newMessageArray = null;
        newMessageArray = new TNotification[totalNotifications];

        Exception[]? newExceptionArray = null;
        newExceptionArray = new Exception[totalExceptions];

        var lastMessageIndex = 0;
        var lastExceptionIndex = 0;

        for (int i = 0; i < processResults.Length; i++)
        {
            var processResult = processResults[i];

            if (newTypeProcessResult is null)
                newTypeProcessResult = processResult.TypeProcessResult;
            else if (newTypeProcessResult == TypeProcessResult.Success && processResult.TypeProcessResult != TypeProcessResult.Success)
                newTypeProcessResult = processResult.TypeProcessResult;

            if (processResult.Notifications is not null)
            {
                Array.Copy(
                    sourceArray: processResult.Notifications,
                    sourceIndex: 0,
                    destinationArray: newMessageArray!,
                    destinationIndex: lastMessageIndex,
                    length: processResult.Notifications.Length
                );

                lastMessageIndex += processResult.Notifications.Length;
            }

            if (processResult.Exceptions is not null)
            {
                Array.Copy(
                    sourceArray: processResult.Exceptions,
                    sourceIndex: 0,
                    destinationArray: newExceptionArray!,
                    destinationIndex: lastExceptionIndex,
                    length: processResult.Exceptions.Length
                );

                lastExceptionIndex += processResult.Exceptions.Length;
            }
        }

        if (newTypeProcessResult is null)
            newTypeProcessResult = outputProcessResult.TypeProcessResult;
        else if (newTypeProcessResult == TypeProcessResult.Success && outputProcessResult.TypeProcessResult != TypeProcessResult.Success)
            newTypeProcessResult = outputProcessResult.TypeProcessResult;

        if (outputProcessResult.Notifications is not null)
        {
            Array.Copy(
                sourceArray: outputProcessResult.Notifications,
                sourceIndex: 0,
                destinationArray: newMessageArray!,
                destinationIndex: lastMessageIndex,
                length: outputProcessResult.Notifications.Length
            );

            lastMessageIndex += outputProcessResult.Notifications.Length;
        }

        if (outputProcessResult.Exceptions is not null)
        {
            Array.Copy(
                sourceArray: outputProcessResult.Exceptions,
                sourceIndex: 0,
                destinationArray: newExceptionArray!,
                destinationIndex: lastExceptionIndex,
                length: outputProcessResult.Exceptions.Length
            );

            lastExceptionIndex += outputProcessResult.Exceptions.Length;
        }

        return new ProcessResult<TNotification, TOutput>(newTypeProcessResult!.Value, outputProcessResult.Output, newMessageArray, newExceptionArray);
    }
    public static ProcessResult<TNotification, TOutput> BuildFromAnotherProcessResult<TAnotherOutput>(TOutput? output,
        params ProcessResult<TNotification, TAnotherOutput>[] processResults)
    {
        var totalNotifications = 0;
        var totalExceptions = 0;

        for (int i = 0; i < processResults.Length; i++)
        {
            totalNotifications += processResults[i].Notifications?.Length ?? 0;
            totalExceptions += processResults[i].Notifications?.Length ?? 0;
        }

        TypeProcessResult? newTypeProcessResult = null;

        TNotification[]? newMessageArray = null;
        newMessageArray = new TNotification[totalNotifications];

        Exception[]? newExceptionArray = null;
        newExceptionArray = new Exception[totalExceptions];

        var lastMessageIndex = 0;
        var lastExceptionIndex = 0;

        for (int i = 0; i < processResults.Length; i++)
        {
            var processResult = processResults[i];

            if (newTypeProcessResult is null)
                newTypeProcessResult = processResult.TypeProcessResult;
            else if (newTypeProcessResult == TypeProcessResult.Success && processResult.TypeProcessResult != TypeProcessResult.Success)
                newTypeProcessResult = processResult.TypeProcessResult;

            if (processResult.Notifications is not null)
            {
                Array.Copy(
                    sourceArray: processResult.Notifications,
                    sourceIndex: 0,
                    destinationArray: newMessageArray!,
                    destinationIndex: lastMessageIndex,
                    length: processResult.Notifications.Length
                );

                lastMessageIndex += processResult.Notifications.Length;
            }

            if (processResult.Exceptions is not null)
            {
                Array.Copy(
                    sourceArray: processResult.Exceptions,
                    sourceIndex: 0,
                    destinationArray: newExceptionArray!,
                    destinationIndex: lastExceptionIndex,
                    length: processResult.Exceptions.Length
                );

                lastExceptionIndex += processResult.Exceptions.Length;
            }
        }

        return new ProcessResult<TNotification, TOutput>(newTypeProcessResult!.Value, output, newMessageArray, newExceptionArray);
    }

    #region Execution Encapsulated

    public static ProcessResult<TNotification, TOutput> Execute(Func<ProcessResult<TNotification, TOutput>> handler)
        => handler();
    public static Task<ProcessResult<TNotification, TOutput>> Execute(Func<Task<ProcessResult<TNotification, TOutput>>> handler)
        => handler();
    public static ProcessResult<TNotification, TOutput> Execute<TInput>(TInput input, Func<TInput, ProcessResult<TNotification, TOutput>> handler)
        => handler(input);
    public static Task<ProcessResult<TNotification, TOutput>> Execute<TInput>(TInput input, Func<TInput, Task<ProcessResult<TNotification, TOutput>>> handler)
        => handler(input);

    #endregion

    #endregion
}
