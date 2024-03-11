using NLog;

namespace Bloggs.MiddleWare
{
    public class ArticleMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
        public ArticleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();
           
            //// Если запрос отправлен на некоторый контроллер
            //if (path.Contains("/article"))
            //{
                
            //    if (context.Request.Method == "POST")
            //    {
            //        var requestForm = await context.Request.ReadFormAsync();
            //        var title = requestForm["title"];
            //        var content = requestForm["content"];

            //        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
            //        {
            //            context.Response.StatusCode = 400;
            //            await context.Response.WriteAsync("The title and content fields are required");
            //            Logger.Warn("The title and content fields are required");
            //        }
            //        //else if (!context.User.Identity.IsAuthenticated)
            //        //{
            //        //    context.Response.StatusCode = 401;
            //        //    context.Response.WriteAsync("No Autharized");
            //        //    Logger.Warn("No Autharized");
            //        //    return;
            //        //}
            //        else await _next(context);
            //    }
            //    else await _next(context);

            //}
             await _next(context); 


        }
    }
}
