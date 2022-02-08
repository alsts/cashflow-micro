using System;
using Polly;

namespace Cashflow.Common.Utils
{
    public static class NetworkUtils
    {
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
