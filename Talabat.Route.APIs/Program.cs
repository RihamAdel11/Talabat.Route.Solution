using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.Route.APIs.Errors;
using Talabat.Route.APIs.Extentions;
using Talabat.Core.Repositries.Contract;
using Talabat.Repositry;
using Talabat.Repositry.Data;
using Talabat.Route.APIs.Helpers;
using StackExchange.Redis;
using Talabat.Repositry.Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Services.AuthServices;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Newtonsoft.Json;

namespace Talabat.Route.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddNewtonsoftJson (Options =>
            {
                Options.SerializerSettings.ReferenceLoopHandling  = ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwagerServices();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );
			
			builder.Services.AddCors(options => { options.AddPolicy("MyPolicy", policyOptions => { policyOptions.AllowAnyHeader().AllowAnyMethod().WithOrigins(builder.Configuration["FrontBaseUrl"]); }); });
           builder.Services.AddAplicationServices();
            builder.Services.AddScoped(typeof(IAuthServices), typeof(AuthService)); 
            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
			builder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);

			});
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //options.Password.RequiredUniqueChars = 2;
                //options.Password.RequireDigit = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
            }).
            AddEntityFrameworkStores <ApplicationIdentityDbContext>();
            builder.Services.AddAuthentication(options => { 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultForbidScheme= JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuser"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:MySecuredAPIUser"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"] ?? string.Empty))
                ,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
            builder.Services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });
			var app = builder.Build();

            using var scope = app.Services.CreateScope();

            var servies = scope.ServiceProvider;
            var _dbContext = servies.GetRequiredService<StoreContext>();
			var _IdentitydbContext = servies.GetRequiredService<ApplicationIdentityDbContext>();
			var loggerFactory = servies.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();//update DataBase
				await StoreContextSeed.SeedAsync(_dbContext);//Data Seeding
				await _IdentitydbContext.Database.MigrateAsync();
                var _usermanager = servies.GetRequiredService<UserManager<ApplicationUser>>();
               
                await ApplicationIdentityDataSeed.SeedUserAsync(_usermanager);

			}
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, " an Error has Occure During Apply Migration");


            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {   
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithRedirects("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseCors("MyPolicy");
            app.MapControllers();

            app.Run();
        }
    }
}
