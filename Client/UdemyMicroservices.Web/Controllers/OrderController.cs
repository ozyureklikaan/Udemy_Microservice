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
            // 1. Yol Senkron İletişim
            // -----------------------
            //var orderStatus = await _orderService.CreateOrder(checkoutInfoInput);

            //if (!orderStatus.IsSuccessful)
            //{
            //    var basket = await _basketService.Get();
            //    ViewBag.basket = basket;
            //    ViewBag.error = orderStatus.Error;
            //    return View();
            //}

            //return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = orderStatus.OrderId });

            // --------------------------------------------------------------------------------

            // 2. Yol Asenkron İletişim
            // -----------------------
            var orderStatus = await _orderService.SuspendOrder(checkoutInfoInput);

            if (!orderStatus.IsSuccessful)
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return View();
            }

            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1, 1000) });
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetOrder());
        }
    }
}
