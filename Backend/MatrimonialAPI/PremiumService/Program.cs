using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using PremiumService.AsyncDataService;
using PremiumService.Data;
using PremiumService.Interfaces;
using PremiumService.Models;
using PremiumService.Repositories;
using PremiumService.Services;
using System.Diagnostics.CodeAnalysis;

namespace PremiumService
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static async Task Main(string[] args)
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
            var kvUri = builder.Configuration.GetConnectionString("keyvaulturi");
            var clientId = builder.Configuration["Azure_Client_ID"];
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential(new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = clientId
            }));
            var connectionstring = await client.GetSecretAsync("PremiumServiceConnectionString");

            #region context
            builder.Services.AddDbContext<PremiumServiceDBContext>(options =>
            {
                options.UseSqlServer(connectionstring.Value.Value);
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

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PremiumServiceDBContext>();
                dbContext.Database.Migrate();
            }

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
