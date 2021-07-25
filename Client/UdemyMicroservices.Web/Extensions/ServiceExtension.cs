using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Handler;
using UdemyMicroservices.Web.Models;
using UdemyMicroservices.Web.Services;
using UdemyMicroservices.Web.Services.Interfaces;

namespace UdemyMicroservices.Web.Extensions
{
    public static class ServiceExtension
    {
        public static void AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceApiSettings = configuration.GetSection("ServiceApiSettings").Get<ServiceApiSettings>();

            services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();

            services.AddHttpClient<IIdentityService, IdentityService>();
            
            services.AddHttpClient<IUserService, UserService>(opt =>
            {
                opt.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
            })
                .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
            
            services.AddHttpClient<ICatalogService, CatalogService>(opt =>
            {
                opt.BaseAddress = new Uri($"{ serviceApiSettings.GatewayBaseUri }{ serviceApiSettings.CatalogAPI.Path }");
            })
                .AddHttpMessageHandler<ClientCredentialTokenHandler>();
            
            services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt =>
            {
                opt.BaseAddress = new Uri($"{ serviceApiSettings.GatewayBaseUri }{ serviceApiSettings.PhotoStockAPI.Path }");
            })
                .AddHttpMessageHandler<ClientCredentialTokenHandler>();
            
            services.AddHttpClient<IBasketService, BasketService>(opt =>
            {
                opt.BaseAddress = new Uri($"{ serviceApiSettings.GatewayBaseUri }{ serviceApiSettings.BasketAPI.Path }");
            })
                .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IDiscountService, DiscountService>(opt =>
            {
                opt.BaseAddress = new Uri($"{ serviceApiSettings.GatewayBaseUri }{ serviceApiSettings.DiscountAPI.Path }");
            })
                .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IPaymentService, PaymentService>(opt =>
            {
                opt.BaseAddress = new Uri($"{ serviceApiSettings.GatewayBaseUri }{ serviceApiSettings.PaymentAPI.Path }");
            })
                .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IOrderService, OrderService>(opt =>
            {
                opt.BaseAddress = new Uri($"{ serviceApiSettings.GatewayBaseUri }{ serviceApiSettings.OrderAPI.Path }");
            })
                .AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }
    }
}
