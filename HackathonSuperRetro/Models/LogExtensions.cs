namespace HackathonSuperRetro.Models;

public static class LogExtensions
{
    public static LogLevel ToInternal(this Uniscale.Core.LogLevel source)
    {
        return source switch
        {
            Uniscale.Core.LogLevel.Trace => LogLevel.Trace,
            Uniscale.Core.LogLevel.Debug => LogLevel.Debug,
            Uniscale.Core.LogLevel.Info => LogLevel.Information,
            Uniscale.Core.LogLevel.Warning => LogLevel.Warning,
            Uniscale.Core.LogLevel.Error => LogLevel.Error,
            Uniscale.Core.LogLevel.Fatal => LogLevel.Critical,
            _ => LogLevel.None
        };
    }
}