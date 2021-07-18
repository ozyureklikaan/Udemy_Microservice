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
    }

    public class ServiceApi
    {
        public string Path { get; set; }
    }
}
