namespace PruebaTecnica.Middleware;

public class ApiKeyMiddleware{
    private const string HeaderName = "X-API-KEY";

    private readonly RequestDelegate _next;
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration){
        _next = next;

        string? apiKey = configuration["ApiKey"];

        if( string.IsNullOrWhiteSpace(apiKey)){
            throw new InvalidOperationException("ApiKey configuration is required.");
        }

        _apiKey = apiKey;
    }

    public async Task InvokeAsync(HttpContext context){
        bool hasApiKey = context.Request.Headers.TryGetValue(HeaderName, out var suppliedApiKey);

        if( !hasApiKey || suppliedApiKey.Count != 1 || !string.Equals( suppliedApiKey[0], _apiKey, StringComparison.Ordinal)){
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            await context.Response.WriteAsJsonAsync(new {error = "Missing or invalid API key"});

            return;
        }

        await _next(context);
    }
}