using System.Diagnostics;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.Logging;

using NeuroMedia.Application.Exceptions;

namespace NeuroMedia.Application.Logging
{
    public class LogRow
    {
        public string Message { get; set; } = string.Empty;
        public Guid EntityId { get; set; }
        public string User { get; set; } = string.Empty;
        public string? StackTrace { get; set; }
        public string? CallerMemberName { get; set; }
        public int? CallerLineNumber { get; set; }
        public string? CallerFilePath { get; set; }
        public string? LogData { get; set; }

        public LogRow(LogRow? logRow = null)
        {
            if (logRow == null)
            {
                return;
            }

            Message = logRow.Message;
            EntityId = logRow.EntityId;
            User = logRow.User;
            StackTrace = logRow.StackTrace;
            CallerMemberName = logRow.CallerMemberName;
            CallerLineNumber = logRow.CallerLineNumber;
            CallerFilePath = logRow.CallerFilePath;
            LogData = logRow.LogData;
        }
    }

    public static class LogRowExtensions
    {
        public static void LogCritical<T>(this ILogger<T> logger, LogRow row, Exception? e = null)
        {
            CheckRowMessageFormat<T>(row);
            UpdateLogRowFromException(row, e);
            s_logCritical(logger, row, e);
        }

        public static void LogDebug<T>(this ILogger<T> logger, LogRow row, Exception? e = null)
        {
            CheckRowMessageFormat<T>(row);
            UpdateLogRowFromException(row, e);
            s_logDebug(logger, row, e);
        }

        public static void LogError<T>(this ILogger<T> logger, LogRow row, Exception? e = null)
        {
            CheckRowMessageFormat<T>(row);
            UpdateLogRowFromException(row, e);
            s_logError(logger, row, e);
        }

        public static void LogInformation<T>(this ILogger<T> logger, LogRow row, Exception? e = null)
        {
            CheckRowMessageFormat<T>(row);
            UpdateLogRowFromException(row, e);
            s_logInformation(logger, row, e);
        }

        public static void LogTrace<T>(this ILogger<T> logger, LogRow row, Exception? e = null)
        {
            CheckRowMessageFormat<T>(row);
            UpdateLogRowFromException(row, e);
            s_logTrace(logger, row, e);
        }

        public static void LogWarning<T>(this ILogger<T> logger, LogRow row, Exception? e = null)
        {
            CheckRowMessageFormat<T>(row);
            UpdateLogRowFromException(row, e);
            s_logWarning(logger, row, e);
        }

        public static void LogErrorAndThrow500<T>(this ILogger<T> logger, LogRow row, string message, Exception? e = null)
        {
            row.Message = message;
            logger.LogError(row, e);

            throw new ClientException(message, e);
        }

        public static void LogWarningAndThrow400<T>(this ILogger<T> logger, LogRow row, string message, Exception? e = null)
        {
            row.Message = message;
            logger.LogWarning(row);

            throw new BadRequestClientException(message, e);
        }

        public static void LogWarningAndThrow404<T>(this ILogger<T> logger, LogRow row, string message, Exception? e = null)
        {
            row.Message = message;
            logger.LogWarning(row);

            throw new NotFoundClientException(message, e);
        }

        public static void LogWarningAndThrow403<T>(this ILogger<T> logger, Exception? e, LogRow row, string message = null)
        {
            row.Message = message;
            logger.LogWarning(row);

            throw new ForbiddenClientException(message, e);
        }

        public static void LogWarningAndThrow401<T>(this ILogger<T> logger, LogRow row, string message, Exception? e = null)
        {
            row.Message = message;
            logger.LogWarning(row);

            throw new UnauthorizedClientException(message, e);
        }

        /// <summary>
        ///     Updates logRow.Callers properties
        /// </summary>
        /// <param name="logRow"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
        public static LogRow UpdateCallerProperties(this LogRow logRow,
            [CallerMemberName] string? callerMemberName = null,
            [CallerFilePath] string? callerFilePath = null,
            [CallerLineNumber] int? callerLineNumber = null)
        {
            logRow.CallerLineNumber = callerLineNumber;
            logRow.CallerFilePath = callerFilePath;
            logRow.CallerMemberName = callerMemberName;

            return logRow;
        }

        private static readonly Action<ILogger, LogRow, Exception?> s_logCritical =
            LoggerMessage.Define<LogRow>(LogLevel.Critical, new EventId(), "{@logRow}");

        private static readonly Action<ILogger, LogRow, Exception?> s_logDebug =
            LoggerMessage.Define<LogRow>(LogLevel.Debug, new EventId(), "{@logRow}");

        private static readonly Action<ILogger, LogRow, Exception?> s_logError =
            LoggerMessage.Define<LogRow>(LogLevel.Error, new EventId(), "{@logRow}");

        private static readonly Action<ILogger, LogRow, Exception?> s_logInformation =
            LoggerMessage.Define<LogRow>(LogLevel.Information, new EventId(), "{@logRow}");

        private static readonly Action<ILogger, LogRow, Exception?> s_logTrace =
            LoggerMessage.Define<LogRow>(LogLevel.Trace, new EventId(), "{@logRow}");

        private static readonly Action<ILogger, LogRow, Exception?> s_logWarning =
            LoggerMessage.Define<LogRow>(LogLevel.Warning, new EventId(), "{@logRow}");

        private static void UpdateLogRowFromException(LogRow row, Exception? e)
        {
            if (e == null)
            {
                return;
            }

            row.StackTrace = e.StackTrace;
            var stackTrace = new StackTrace(e, true);

            foreach (var frame in stackTrace.GetFrames()
                         .Where(frame => frame.GetMethod() != null))
            {
                row.CallerFilePath = frame.GetFileName();
                row.CallerLineNumber = frame.GetFileLineNumber();
                row.CallerMemberName = frame.GetMethod()!.Name;
            }
        }

        private static void CheckRowMessageFormat<T>(LogRow row)
        {
            if (string.IsNullOrEmpty(row.Message))
            {
                return;
            }

            var startWith = typeof(T).Name + ": ";
            row.Message = row.Message.StartsWith(startWith, StringComparison.Ordinal)
                ? row.Message
                : startWith + row.Message;
        }
    }
}
