using Microsoft.Extensions.Logging;
using RTCodingExercise.Microservices.Controllers;
using System;

namespace WebMVC.UnitTests.Fakes
{
    internal class FakeLogger : ILogger<HomeController>
    {
        public string LastLog = string.Empty;
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            LastLog = state.ToString();
        }
    }
}
