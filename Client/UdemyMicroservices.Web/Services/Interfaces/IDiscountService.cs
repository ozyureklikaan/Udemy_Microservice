using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models.Discounts;

namespace UdemyMicroservices.Web.Services.Interfaces
{
    public interface IDiscountService
    {
        Task<DiscountViewModel> GetDiscount(string discountCode);
    }
}
