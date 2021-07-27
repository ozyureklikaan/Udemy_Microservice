using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models.Orders;

namespace UdemyMicroservices.Web.Services.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderViewModel>> GetOrder();

        /// <summary>
        /// Senkron İletişim. Direk order mikroservisine istek yapılacak.
        /// </summary>
        /// <param name="checkoutInfoInput"></param>
        /// <returns></returns>
        Task<OrderCreatedViewModel> CreateOrder(CheckoutInfoInput checkoutInfoInput);
        
        /// <summary>
        /// Asenkron iletişim. Sipariş bilgileri rabbitMQ'ya gönderilecek.
        /// </summary>
        /// <param name="checkoutInfoInput"></param>
        /// <returns></returns>
        Task<OrderSuspendViewModel> SuspendOrder(CheckoutInfoInput checkoutInfoInput);
    }
}
