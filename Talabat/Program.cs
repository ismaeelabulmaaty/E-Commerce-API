using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Text;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Errors;
using Talabat.Extentions;
using Talabat.Helpar;
using Talabat.MiddleWare;
using Talabat.Repository.Data;
using Talabat.Repository.Data.Identity;
using Talabat.Service;

namespace Talabat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            //StoreContext dbContext = new StoreContext();
            //await dbContext.Database.MigrateAsync();

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Dependancy Injection

            builder.Services.AddControllers()/*.AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })*/;
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwagerServices();

            builder.Services.AddDbContext<StoreContext>( options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //builder.Services.AddDbContext<AppIdentityDbContext>(optionBuilder =>
            //{
            //    optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")); 
            //});
            //builder.Services.AddScoped<IGenericRepository<Product>,GenericRepository<Product>>(); 

            //ApplicationServicesEctension.AddApplicationServices(builder.Services);
            builder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
            {
                var connection = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });


            builder.Services.AddApplicationServices();

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {

            }).AddEntityFrameworkStores<StoreContext>();


            builder.Services.AddScoped(typeof(IAuthSrvices),typeof(AuthServices));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(/*JwtBearerDefaults.AuthenticationScheme , */ options =>
            {

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssured"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:AuthKey"] ?? string.Empty)),

                };

            });

            builder.Services.AddCors(Options =>
            {
                Options.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyHeader();
                    options.AllowAnyMethod();
                    options.AllowAnyOrigin();
                });
            });

            #endregion
            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
             var _dbContext=services.GetRequiredService<StoreContext>();
             //var _IdentityDbContext = services.GetRequiredService<StoreContext>();
            var logarfactory = services.GetRequiredService<ILoggerFactory>();
            var _userManger = services.GetRequiredService<UserManager<AppUser>>(); //Explicitly
            try
            {

                await _dbContext.Database.MigrateAsync();            //Update Database
                await StoreContextSeeding.SeedAsync(_dbContext);    // Data Seeding
                                                                    // await _IdentityDbContext.Database.MigrateAsync();  //Update Database
                await AppIdentityDbContextSeed.SeedUserAsync(_userManger); // Data Seeding

            }
            catch (Exception ex)
            {

                var logar = logarfactory.CreateLogger<Program>();
                logar.LogError(ex, "An Error Occurred During Apply Migration");
            }
            // Configure the HTTP request pipeline.
            #region Middlewares

            app.UseMiddleware<ExceptionMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("MyPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers(); 

            #endregion

            app.Run();
        }
    }
}
