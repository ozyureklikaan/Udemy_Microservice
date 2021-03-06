using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyMicroservices.Web.Models
{
    public class ServiceApiSettings
    {
        public string GatewayBaseUri { get; set; }
        public string IdentityBaseUri { get; set; }
        public string PhotoStockUri { get; set; }
        public ServiceApi CatalogAPI { get; set; }
        public ServiceApi PhotoStockAPI { get; set; }
        public ServiceApi BasketAPI { get; set; }
        public ServiceApi DiscountAPI { get; set; }
        public ServiceApi PaymentAPI { get; set; }
        public ServiceApi OrderAPI { get; set; }
    }

    public class ServiceApi
    {
        public string Path { get; set; }
    }
}
