using DBContex.Models;
using DBContex.Repository;
using NLog;

namespace Bloggs.MiddleWare
{
    public class TagMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
        public TagMiddleware(RequestDelegate next)
        {
            _next = next;

        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();

            // Если запрос отправлен на некоторый контроллер
            if (path.Contains("/tags"))
            {
                if (context.Request.Method == "POST")
                {
                    var requestForm = await context.Request.ReadFormAsync();
                    var tag =requestForm["tagName"];
               

                    if (string.IsNullOrWhiteSpace(tag))
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync("The Tag exists");
                        Logger.Warn("The Tag exists");
                    }
                    //else if (!context.User.Identity.IsAuthenticated)
                    //{
                    //    context.Response.StatusCode = 401;
                    //    context.Response.WriteAsync("No Autharized");
                    //    Logger.Warn("No Autharized");
                    //    return;
                    //}
                    else await _next(context);
                }
                else await _next(context);

            }
            else await _next(context);

        }
    }
}
