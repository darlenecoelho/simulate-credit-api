namespace SimulateCredit.Application.Exceptions;

public class SimulationException : Exception
{
    public SimulationException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
