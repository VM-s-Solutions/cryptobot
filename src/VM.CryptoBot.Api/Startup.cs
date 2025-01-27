using VM.CryptoBot.Api.Middlewares;
using VM.CryptoBot.Config;

namespace VM.CryptoBot.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection services, IHostEnvironment env)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpContextAccessor();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        
        services.AddCoreBindings(configuration, env);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (!env.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}