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

namespace Talabat.Route.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           builder.Services.AddSwagerServices();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );
           builder.Services.AddAplicationServices();
			builder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
			{
				var connection = builder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);

			});
			var app = builder.Build();

            using var scope = app.Services.CreateScope();

            var servies = scope.ServiceProvider;
            var _dbContext = servies.GetRequiredService<StoreContext>();

            var loggerFactory = servies.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync();//update DataBase
                await StoreContextSeed.SeedAsync(_dbContext);//Data Seeding
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


            app.MapControllers();

            app.Run();
        }
    }
}
