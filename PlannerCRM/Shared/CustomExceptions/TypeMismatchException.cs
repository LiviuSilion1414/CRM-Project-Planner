namespace PlannerCRM.Shared.CustomExceptions;

public class TypeMismatchException : Exception
{
    public TypeMismatchException(string message)
        : base(message)
    { }
}

