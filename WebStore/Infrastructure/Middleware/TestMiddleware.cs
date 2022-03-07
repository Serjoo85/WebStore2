namespace WebStore.Infrastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            // Обработка информации из Context.Request/Response
            await _next(context);
            // Обработка результата обработки запроса.

        }
    }
}
