using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Api.Data;
using SocialNetwork.Api.Middlewares;
using SocialNetwork.Api.Services;
using System.Text;

namespace SocialNetwork.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ExceptionHandlingMiddleware>();

            services.AddDbContext<AccDbContext>(options =>
                options.UseNpgsql(Configuration["CONNECTION_STRING"]));

            services.AddScoped<IUserService, UserService>();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["ISSUER"],
                        ValidAudience = Configuration["AUDIENCE"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                            .GetBytes(Configuration["SECRET_KEY"]))
                    };
                });

            services.AddControllers()
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.PropertyNamingPolicy = null);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}