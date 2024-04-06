using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TodoApi.Dato;
using codemazeapicontroler.Services.Ad;
using Newtonsoft.Json;
using System.IO;


namespace TodoApi
{
    public class Startup
    {
     private readonly IWebHostEnvironment _env;
        public Service _service {get; set;}
        public IConfiguration Configuration { get; }
         public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
        _env=env;
        }
       

   

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
     public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<Mycontext>(options =>
    {
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
    });
    
    services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyOrigin",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
   public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{

   
  app.UseCors("AllowAnyOrigin");
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<Mycontext>();
        _service = new Service(dbContext);

        endpoints.MapGet("/", async context =>
        {
            try
            {
                var items = await dbContext.Items.ToListAsync();
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(items));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving all notes: {ex.Message}");
                throw;
            }
        });

        endpoints.MapPost("/", async context =>
        {
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

            // המרת המחרוזת לאובייקט Item
            var newItem = JsonConvert.DeserializeObject<TodoApi.Dato.Item>(requestBody);
            // שליחת הפריט למתודה המטפלת ביצירת פריט
await context.Response.WriteAsync(await _service.CreateNote(newItem));
        });

    endpoints.MapPut("/{id}", async context =>
{
    if (context.Request.RouteValues["id"] is string idString && int.TryParse(idString, out int id))
    {
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();

        // המרת המחרוזת לאובייקט Item
        var newItem = JsonConvert.DeserializeObject<TodoApi.Dato.Item>(requestBody);

        // שליחת הפריט וה־ID למתודה המטפלת בעדכון פריט
        await context.Response.WriteAsync(await _service.UpdateNote(newItem, id));
    }
    else
    {
        // טיפול במקרה בו ה־ID אינו מספר שלם תקין
        context.Response.StatusCode = 400; // Bad Request
        await context.Response.WriteAsync("Invalid ID format");
    }
});
        endpoints.MapDelete("/{id}", async context =>
        {
            if (context.Request.RouteValues["id"] is string idString && int.TryParse(idString, out int id))
            {
                await context.Response.WriteAsync(await _service.DeleteNote(id));
            }
            
    
        });

        endpoints.MapMethods("/options-or-head", new[] { "OPTIONS", "HEAD" }, async context =>
        {
            await context.Response.WriteAsync("This is an options or head request");
        });
    });
}
    }
}
