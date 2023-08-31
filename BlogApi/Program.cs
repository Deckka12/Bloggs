using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using DBContex.Repository;
using DBContex.Models;
using System.Text.Json.Serialization;

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
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "HomeApi", Version = "v1" }); }); 
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();

            app.UseAuthorization();
            
                app.MapControllers();
            
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HomeApi v1"));
            

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            // Сопоставляем маршруты с контроллерами
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.Run();
        }
    }
}