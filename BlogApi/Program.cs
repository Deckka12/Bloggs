using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using DBContex.Repository;
using DBContex.Models;

namespace BlogApi
{
    public class Program
    {
        public static void Main (string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<Context>();
            builder.Services.AddIdentity<User, IdentityRole>()
                            .AddEntityFrameworkStores<Context>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ITagRepository, TagRepository>();
            builder.Services.AddTransient<ICommentRepository, CommentRepository>();
            builder.Services.AddTransient<IArticleRepository, ArticleRepository>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthorization();
          
            app.MapControllers();

            app.Run();
        }
    }
}