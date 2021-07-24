using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models.Baskets;

namespace UdemyMicroservices.Web.Services.Interfaces
{
    public interface IBasketService
    {
        Task<BasketViewModel> Get();
        Task AddBasketItem(BasketItemViewModel basketItemViewModel);
        Task<bool> ApplyDiscount(string discountCode);
        Task<bool> CancelApplyDiscount();
        Task<bool> SaveOrUpdate(BasketViewModel basketViewModel);
        Task<bool> Delete();
        Task<bool> RemoveBasketItem(string courseId);
    }
}
