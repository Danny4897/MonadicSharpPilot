using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonadicPilot.Backend.Infrastructure.Configuration;

namespace MonadicPilot.Backend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add framework services
        services.AddControllers()
            .AddNewtonsoftJson(); // Already referenced in .csproj

        // Add Entity Framework
        // services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

        // Add MonadicSharp and domain services
        services.AddMonadicSharp()
               .AddDomainServices()
               .AddApplicationServices();

        // Add API documentation
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Add CORS for frontend integration
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200") // Angular default port
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseCors("AllowFrontend");
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
