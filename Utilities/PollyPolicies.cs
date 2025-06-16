using Polly;

namespace Nutrition_Tracker.Utilities;

public static class PollyPolicies
{
    //Retry Policy for HttpRequestException and non-successful HTTP status codes. 5 retries with exponential backoff
    public static readonly IAsyncPolicy<HttpResponseMessage> HttpRequestRetry = Policy
        .Handle<HttpRequestException>()
        .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
        .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100));
}