using OVB.Demos.Eschody.Libraries.ProcessResultContext.Enums;

namespace OVB.Demos.Eschody.Libraries.ProcessResultContext.Exceptions;

public sealed class ProcessResultException : Exception
{
    public ProcessResultException(string message) : base(message) { }

    public static void ThrowExceptionIfProcessResultTypeIsNotValid(
        TypeProcessResult typeProcessResult)
    {
        if (Enum.IsDefined(typeProcessResult) is false)
            throw new ProcessResultException("The type of process result that you send is not valid.");
    }
}