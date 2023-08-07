using Polly.Extensions.Http;
using Polly;
using Microsoft.Extensions.Options;
using WebhookSender.Models;
using Microsoft.AspNetCore.Hosting;

namespace WebhookSender.Extensions
{
    public static class Retry
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(IServiceProvider services)
        {
            var policyOptions = services.GetRequiredService<IOptions<WebhookPolicyOptions>>().Value;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var random = new Random();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(
                    policyOptions.RetryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                    + TimeSpan.FromMilliseconds(random.Next(0, 1000)),
                    onRetry: (outcome, timespan, retryAttempt, context) =>
                    {
                        logger.LogWarning($"Delaying for {timespan.TotalSeconds} seconds, then making retry {retryAttempt}");
                    });
        }

        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IServiceProvider services)
        {
            var policyOptions = services.GetRequiredService<IOptions<WebhookPolicyOptions>>().Value;
            var logger = services.GetRequiredService<ILogger<Program>>();

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    policyOptions.RetryCount,
                    TimeSpan.FromSeconds(policyOptions.BreakDuration),
                    onBreak: (outcome, timespan, context) =>
                    {
                        logger.LogCritical($"Circuit breaker is now open for {timespan.TotalSeconds} seconds...");
                    },
                    onReset: (context) =>
                    {
                        logger.LogInformation("Circuit breaker is now closed again");
                    },
                    onHalfOpen: () =>
                    {
                        logger.LogInformation("Circuit breaker is half-open, the next operation will be allowed...");
                    });
        }

    }
}
