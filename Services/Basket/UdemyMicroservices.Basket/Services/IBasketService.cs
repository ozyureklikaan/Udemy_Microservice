﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Basket.Dtos;
using UdemyMicroservices.Shared.Dtos;

namespace UdemyMicroservices.Basket.Services
{
    public interface IBasketService
    {
        Task<Response<BasketDto>> GetBasket(string userId);

        Task<Response<bool>> SaveOrUpdate(BasketDto basketDto);

        Task<Response<bool>> Delete(string userId);
    }
}
