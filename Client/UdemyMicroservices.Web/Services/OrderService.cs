using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UdemyMicroservices.Shared.Dtos;
using UdemyMicroservices.Shared.Services;
using UdemyMicroservices.Web.Models.FakePayments;
using UdemyMicroservices.Web.Models.Orders;
using UdemyMicroservices.Web.Services.Interfaces;

namespace UdemyMicroservices.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentService _paymentService;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public OrderService(HttpClient httpClient, IPaymentService paymentService, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _sharedIdentityService = sharedIdentityService ?? throw new ArgumentNullException(nameof(sharedIdentityService));
        }

        public async Task<List<OrderViewModel>> GetOrder()
        {
            var response = await _httpClient.GetFromJsonAsync<Response<List<OrderViewModel>>>("orders");

            return response.Data.OrderByDescending(x => x.Id).ToList();
        }

        public async Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();

            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice
            };

            var responsePayment = await _paymentService.ReveivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderCreatedViewModel()
                {
                    Error = "Ödeme alınamadı",
                    IsSuccessful = false
                };
            }

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput()
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                    Line = checkoutInfoInput.Line
                }
            };

            basket.BasketItems.ForEach(x =>
            {
                orderCreateInput.OrderItems.Add(new OrderItemCreateInput()
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    PictureUrl = "",
                    ProductName = x.CourseName
                });
            });

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);

            if (!response.IsSuccessStatusCode)
            {
                return new OrderCreatedViewModel()
                {
                    Error = "Sipariş oluşturulamadı",
                    IsSuccessful = false
                };
            }

            var orderCreatedViewModel = await response.Content.ReadFromJsonAsync<Response<OrderCreatedViewModel>>();

            orderCreatedViewModel.Data.IsSuccessful = true;

            await _basketService.Delete();

            return orderCreatedViewModel.Data;
        }

        public async Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput)
        {
            var basket = await _basketService.Get();

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address = new AddressCreateInput()
                {
                    Province = checkoutInfoInput.Province,
                    District = checkoutInfoInput.District,
                    Street = checkoutInfoInput.Street,
                    ZipCode = checkoutInfoInput.ZipCode,
                    Line = checkoutInfoInput.Line
                }
            };

            basket.BasketItems.ForEach(x =>
            {
                orderCreateInput.OrderItems.Add(new OrderItemCreateInput()
                {
                    ProductId = x.CourseId,
                    Price = x.GetCurrentPrice,
                    PictureUrl = "",
                    ProductName = x.CourseName
                });
            });

            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutInfoInput.CardName,
                CardNumber = checkoutInfoInput.CardNumber,
                Expiration = checkoutInfoInput.Expiration,
                CVV = checkoutInfoInput.CVV,
                TotalPrice = basket.TotalPrice,
                Order = orderCreateInput
            };

            var responsePayment = await _paymentService.ReveivePayment(paymentInfoInput);

            if (!responsePayment)
            {
                return new OrderSuspendViewModel()
                {
                    Error = "Ödeme alınamadı",
                    IsSuccessful = false
                };
            }

            await _basketService.Delete();

            return new OrderSuspendViewModel() { IsSuccessful = true };
        }
    }
}
