using System;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Customisation.Sandbox
{
    public static class TraceLog
    {
        public static void LogInfo(ILogger logger, string message)
        {
            logger.Log(LogLevel.Information, message);
        }

        public static void LogDebug(ILogger logger, string message)
        {
            logger.Log(LogLevel.Debug, message);
        }

        public static void LogCritical(ILogger logger, string message)
        {
            logger.Log(LogLevel.Critical, message);
        }
    }

    public class TestLogging
    {
        public void FirstTest()
        {
            var l = new SecondCustomLogger();

            TraceLog.LogCritical(l, "This is a test message");
        }
    }

    public class CustomLogger : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"LogLevel = {logLevel}; Message = {state}");
        }
        
        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }

    public class SecondCustomLogger : ILogger
    {
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"LogLevel was {logLevel}; and Message was {state}");
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }
    }
}
