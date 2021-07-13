using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyMicroservices.Web.Models
{
    public class ClientSettings
    {
        public Client WebMvcClient { get; set; }
        public Client WebMvcClientForUser { get; set; }
    }

    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
