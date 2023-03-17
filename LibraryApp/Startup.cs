using Application.Mapper;
using Application.Services;
using Application.Services.Interfaces;
using Domain.Interfaces;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.BackgroundJobs;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    private IConfiguration Configuration { get; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        services.AddAutoMapper(typeof(AppMappingProfile));
        services.AddHangfire(h => h.UsePostgreSqlStorage(Configuration.GetConnectionString("HFConnection")));
        services.AddHangfireServer();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBorrowHistoryService, BorrowHistoryService>();
        services.AddScoped<IBorrowHistoryRepository, BorrowHistoryRepository>();
        services.AddScoped<IUserRatingJobScheduler, UserRatingJobScheduler>();
        services.AddScoped<IUserRatingJobService, UserRatingJobService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        env.IsDevelopment();

        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        
        app.UseAuthorization();

        app.UseHangfireDashboard("/dashboard");

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}