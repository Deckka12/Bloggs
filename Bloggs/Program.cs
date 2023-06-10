using DBContex.Models;
using DBContex.Repository;
using Bloggs.Services;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<Context>();
builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<Context>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<ITagRepository, TagRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
builder.Services.AddTransient<IArticleServices, ArticleServices>();

builder.Services.AddControllersWithViews();
var app = builder.Build();
//string connection = builder.Configuration.GetConnectionString("DefaultConnection");

// добавляем контекст ApplicationContext в качестве сервиса в приложение


// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    endpoints.MapControllerRoute(
        name: "users",
        pattern: "Users/{action=Login}",
        defaults: new { controller = "Users" });

    // этот маршрут настраивает адрес страницы Logout
    endpoints.MapControllerRoute(
        name: "logout",
        pattern: "Users/Logout",
        defaults: new { controller = "Users", action = "Logout" });
});
app.UseExceptionHandler("/Home/Error");
app.Run();
