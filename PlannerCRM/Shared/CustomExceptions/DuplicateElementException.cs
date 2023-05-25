namespace PlannerCRM.Shared.CustomExceptions;

public class DuplicateElementException : Exception
{
    public DuplicateElementException(string message)
        : base(message)
    { }
}

