namespace GreenswampRazorPages
{
    public class HttpRequestLogger
    {
        private readonly RequestDelegate _nextHandler;
        private readonly ILogger<HttpRequestLogger> _logger;

        public HttpRequestLogger(RequestDelegate next, ILogger<HttpRequestLogger> logger)
        {
            _nextHandler = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            _logger.LogInformation(
                "Incoming request: {HttpMethod} {RequestPath} at {TimestampUtc}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"));

            await _nextHandler(httpContext);
        }
    }
}