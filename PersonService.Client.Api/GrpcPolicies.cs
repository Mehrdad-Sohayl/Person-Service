
using System;
using System.Net.Http;
using Polly;
using Polly.Timeout;

namespace PersonService.Client.Api
{
    public static class GrpcPolicies
    {
        /// <summary>
        /// Retry policy – 3 attempts with exponential back‑off.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            => Policy<HttpResponseMessage>.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        /// <summary>
        /// Circuit breaker – open after 5 consecutive failures for 1 minute.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
            => Policy<HttpResponseMessage>.Handle<Exception>()
                .CircuitBreakerAsync(5, TimeSpan.FromMinutes(1));

        /// <summary>
        /// Timeout policy – configurable per client.
        /// </summary>
        public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int seconds)
            => Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(seconds));
    }
}

