using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models.Orders;
using UdemyMicroservices.Web.Services.Interfaces;

namespace UdemyMicroservices.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly IOrderService _orderService;

        public OrderController(IBasketService basketService, IOrderService orderService)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();

            ViewBag.basket = basket;

            return View(new CheckoutInfoInput());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutInfoInput checkoutInfoInput)
        {
            var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);

            if (!orderStatus.IsSuccessful)
            {
                ViewBag.error = orderStatus.Error;
                return View();
            }

            return RedirectToAction(nameof(SuccessfulCheckout), new { orderStatus.OrderId });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
