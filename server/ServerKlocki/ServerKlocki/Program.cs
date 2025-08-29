using ServerKlocki.Examples;
using ServerKlocki.Database;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using ServerKlocki.Hubs;
using Microsoft.AspNetCore.SignalR;
using ServerKlocki.Providers;

namespace ServerKlocki
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.ExampleFilters();
                options.EnableAnnotations();
            });
            
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    )
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && 
                            path.StartsWithSegments("/lobby") ||
                            path.StartsWithSegments("/mathmaker"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

            builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            builder.Services.AddAuthorization();

            // swagger examples
            builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

            builder.Services.AddSignalR();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                db.Database.EnsureCreated();
                DatabaseSeeder.Seed(db);
            }

            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapGet("/", () => "Hello World!");
            app.MapPost("/", () => "Hello World! - post methodS");

            app.MapHub<MatchmakingHub>("/matchmaker").RequireAuthorization();
            app.MapHub<LobbyHub>("/lobby").RequireAuthorization();

            //app.UseHttpsRedirection();
            app.Run();
        }
    }
}
