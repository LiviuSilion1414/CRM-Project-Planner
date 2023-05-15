namespace PlannerCRM.Server.CustomExceptions;

public class DuplicateElementException : Exception
{
    public DuplicateElementException(string message)
        : base(message)
    { }
}

