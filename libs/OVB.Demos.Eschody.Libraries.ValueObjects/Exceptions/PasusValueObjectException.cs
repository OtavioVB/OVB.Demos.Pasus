namespace OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

public sealed class PasusValueObjectException : Exception
{
    private PasusValueObjectException(string message) : base(message) { }

    public static void ThrowExceptionIfTheResourceIsNotValid(bool isValid)
    {
        if (isValid == false)
            throw new PasusValueObjectException("O recurso a ser obtido não é valido.");
    }

    public static PasusValueObjectException ExceptionFromActivityNull
        = new PasusValueObjectException("The activity is null for tracing.");
}
