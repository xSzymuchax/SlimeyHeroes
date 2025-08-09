using ServerKlocki.Examples;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

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

            // swagger examples
            builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.MapControllers();
            app.MapGet("/", () => "Hello World!");

            //app.UseHttpsRedirection();
            app.Run();
        }
    }
}
