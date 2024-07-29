using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProfileService.Data;
using ProfileService.Interfaces;
using ProfileService.Models;
using ProfileService.Repositories;
using ProfileService.Services;
using System.Text;

namespace ProfileService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #region context
            builder.Services.AddDbContext<ProfileServiceDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
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
            #endregion

            #region services
            builder.Services.AddScoped<IProfileService, UserProfileService>();
            #endregion

            var app = builder.Build();

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
