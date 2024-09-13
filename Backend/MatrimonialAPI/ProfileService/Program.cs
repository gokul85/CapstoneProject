using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProfileService.AsyncDataServices;
using ProfileService.Data;
using ProfileService.Interfaces;
using ProfileService.Models;
using ProfileService.Repositories;
using ProfileService.Services;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ProfileService
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
            var connectionstring = await client.GetSecretAsync("ProfileServiceConnectionString");

            #region context
            builder.Services.AddDbContext<ProfileServiceDBContext>(options =>
            {
                options.UseSqlServer(connectionstring.Value.Value);
            });
            #endregion

            #region repositories
            builder.Services.AddScoped<IRepository<int, UserProfile>, UserProfileRepository>();
            builder.Services.AddScoped<IRepository<int, BasicInfo>, BasicInfoRepository>();
            builder.Services.AddScoped<IRepository<int, Address>, AddressRepository>();
            builder.Services.AddScoped<IRepository<int, Careers>, CareersRepository>();
            builder.Services.AddScoped<IRepository<int, Educations>, EducationsRepository>();
            builder.Services.AddScoped<IRepository<int, FamilyInfo>, FamilyInfoRepository>();
            builder.Services.AddScoped<IRepository<int, Lifestyle>, LifeStyleRepository>();
            builder.Services.AddScoped<IRepository<int, PhysicalAttributes>, PhysicalAttributesRepository>();
            builder.Services.AddScoped<IRepository<int, PartnerPreference>, PartnerPreferenceRepository>();
            builder.Services.AddScoped<IRepository<int, ProfileImages>, ProfileImagesRepository>();
            #endregion

            #region services
            builder.Services.AddSingleton(new BlobServiceClient("BlobEndpoint=https://matrimonialapi.blob.core.windows.net/;QueueEndpoint=https://matrimonialapi.queue.core.windows.net/;FileEndpoint=https://matrimonialapi.file.core.windows.net/;TableEndpoint=https://matrimonialapi.table.core.windows.net/;SharedAccessSignature=sv=2022-11-02&ss=bfqt&srt=sco&sp=rwdlacupiytfx&se=2024-08-30T15:14:44Z&st=2024-07-30T07:14:44Z&spr=https,http&sig=BY5nDjGhKZMV3%2B871c25XfhcdYfPeAxixelniw9PtsQ%3D"));
            builder.Services.AddSingleton<RabbitMQPublisher>();
            builder.Services.AddScoped<FileUploadService>();
            builder.Services.AddScoped<IProfileService, UserProfileService>();
            builder.Services.AddScoped<ISearchService, SearchService>();
            builder.Services.AddSingleton<RabbitMQConsumer>();
            builder.Services.AddHostedService(sp => sp.GetRequiredService<RabbitMQConsumer>());
            #endregion

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ProfileServiceDBContext>();
                dbContext.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
