using Microsoft.Extensions.Logging;
using SimulateCredit.Application.Ports.Outgoing;

namespace SimulateCredit.Infrastructure.Logging;

public sealed class AuditLogger : IAuditLogger
{
    private readonly ILogger<AuditLogger> _logger;

    public AuditLogger(ILogger<AuditLogger> logger)
        => _logger = logger;

    public void LogInformation(string message, params object[] args)
        => _logger.LogInformation(message, Sanitize(args));

    public void LogError(Exception exception, string message, params object[] args)
        => _logger.LogError(exception, message, Sanitize(args));

    private object[] Sanitize(object[] args)
    {
        var sanitized = new object[args.Length];
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] is string s && s.Contains("@"))
                sanitized[i] = MaskEmail(s);
            else
                sanitized[i] = args[i];
        }
        return sanitized;
    }

    private static string MaskEmail(string email)
    {
        var parts = email.Split('@');
        var user = parts[0];
        var domain = parts[1];
        var prefix = user.Length <= 3
                    ? user
                    : user.Substring(0, 3);
        return $"{prefix}***@{domain}";
    }
}