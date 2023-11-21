using Microsoft.AspNetCore.Mvc;
using Talabat.APIS.Errors;
using Talabat.APIS.Helpers;
using Talabat.core;
using Talabat.core.Repositories;
using Talabat.core.Services;
using Talabat.repository;
using Talabat.services;

namespace Talabat.APIS.Extensions
{
    public static class ApplicationServicesException
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddScoped<IPaymentServices , PaymentService>();
            Services.AddScoped(typeof(IUnitOfWork) , typeof (UnitOfWork));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (ActionContext) =>
                {
                    //ModelState => Dic [KeyValuePair]
                    //Key => Name of Pairs
                    //Value => Error

                    var errors = ActionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                        .SelectMany(P => P.Value.Errors)
                        .Select(E => E.ErrorMessage)
                        .ToArray();
                    var validationerrorresponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationerrorresponse);
                };
            });
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            return Services;

        }
    }
}
