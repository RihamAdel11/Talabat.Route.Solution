using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.Core.Repositries.Contract;
using Talabat.Repositry;
using Talabat.Repositry.Data;
using Talabat.Route.APIs.Helpers;

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
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );
            builder.Services.AddScoped(typeof(IGenericRepositry<>), typeof(GenericRepositry<>));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0).
                    SelectMany(p => p.Value.Errors).Select(E => E.ErrorMessage).ToList();
                    var response = new APIValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
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
