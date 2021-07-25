using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models.FakePayments;

namespace UdemyMicroservices.Web.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ReveivePayment(PaymentInfoInput paymentInfoInput);
    }
}
