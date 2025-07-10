namespace SimulateCredit.Application.Ports.Outgoing;

public interface IAuditLogger
{
    void LogInformation(string message, params object[] args);
    void LogError(Exception exception, string message, params object[] args);
}