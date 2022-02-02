using System;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Polly;

namespace AccountService.Util.Helpers
{
    public static class Utils
    {
        public static bool IsPasswordValid(string password)
        {
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$");

            return passwordRegex.IsMatch(password);
        }
        public static void ConfigureLogs(ILoggingBuilder logBuilder)
        {
            logBuilder.ClearProviders(); // removes all providers from LoggerFactory
            logBuilder.AddConsole();  
            logBuilder.AddTraceSource("Information, ActivityTracing"); // Add Trace listener provider
            logBuilder.AddSimpleConsole(options =>
            {
                options.IncludeScopes = false;
                options.SingleLine = true;
                options.TimestampFormat = "[MM/dd/yyyy HH:mm:ss] ";
            });
        }
        
        public static void TryConnecting<T>(
            int numberOfRetries,
            int delayInSeconds,
            Action retriableAction,
            Action<int> beforeEachRetryAction,
            Action fallBackAction) where T : Exception
        {
            var waitAndRetryPolicy = Policy
                .Handle<T>()
                .WaitAndRetry(
                    numberOfRetries,
                    _ => TimeSpan.FromSeconds(delayInSeconds),
                    (_, _, retryCount, _) => beforeEachRetryAction(retryCount));

            Policy
                .Handle<Exception>()
                .Fallback(fallBackAction)
                .Wrap(waitAndRetryPolicy)
                .Execute(retriableAction);
        }
    }
}
