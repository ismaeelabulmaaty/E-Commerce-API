using Microsoft.AspNetCore.Mvc;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Errors;
using Talabat.Helpar;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.Extentions
{
    public static class ApplicationServicesEctension
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IResponseCachService , ResponseCacheService>();
            services.AddScoped(typeof(IOrderService), typeof(OrderService));
            services.AddScoped<IUnitOfWork , UnitOfWork>();
            services.AddScoped<IpymentService , PymentService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));


            services.AddAutoMapper(typeof(MappingProfil));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    var errors = ActionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                                       .SelectMany(p => p.Value.Errors)
                                                       .Select(E => E.ErrorMessage)
                                                       .ToList();


                    var response = new ApiValidationErrorssResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(response);
                };



            });

            services.AddScoped(typeof(IBasketRepository), typeof(RepositoryBasket));

            return services;

        }



        public static void AddSwagerServices(this IServiceCollection services)
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


        }

    }
}
