using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyMicroservices.Shared.Services
{
    public interface ISharedIdentityService
    {
        string GetUserId { get; }
    }
}
