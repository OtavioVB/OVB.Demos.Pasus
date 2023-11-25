namespace OVB.Demos.Eschody.Libraries.ValueObjects.Exceptions;

public sealed class EschodyValueObjectException : Exception
{
    private EschodyValueObjectException(string message) : base(message) { }

    public static void ThrowExceptionIfTheResourceIsNotValid(bool isValid)
    {
        if (isValid == false)
            throw new EschodyValueObjectException("O recurso a ser obtido não é valido.");
    }
}
