using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Web.Models;

namespace UdemyMicroservices.Web.Helpers
{
    public class PhotoHelper
    {
        private readonly ServiceApiSettings _serviceApiSettings;

        public PhotoHelper(IOptions<ServiceApiSettings> serviceApiSettings)
        {
            _serviceApiSettings = serviceApiSettings.Value ?? throw new ArgumentNullException(nameof(serviceApiSettings));
        }

        public string GetPhotoStockUrl(string photoUrl)
        {
            return $"{ _serviceApiSettings.PhotoStockUri }/photos/{ photoUrl }";
        }
    }
}
