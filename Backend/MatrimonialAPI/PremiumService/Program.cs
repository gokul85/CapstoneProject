using Microsoft.EntityFrameworkCore;
using PremiumService.AsyncDataService;
using PremiumService.Data;
using PremiumService.Interfaces;
using PremiumService.Models;
using PremiumService.Repositories;
using PremiumService.Services;

namespace PremiumService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();

            // Add services to the container.

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region context
            builder.Services.AddDbContext<PremiumServiceDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
            });
            #endregion

            #region repositories
            builder.Services.AddScoped<IRepository<int, Payments>, PaymentsRepository>();
            builder.Services.AddScoped<IRepository<int, ContactViews>, ContactViewsRepository>();
            #endregion

            #region services
            builder.Services.AddSingleton<RabbitMQPublisher>();
            builder.Services.AddScoped<IPremiumUserService, PremiumUserService>();
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
