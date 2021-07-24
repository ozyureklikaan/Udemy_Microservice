using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemyMicroservices.Basket.Dtos;
using UdemyMicroservices.Basket.Services;
using UdemyMicroservices.Shared.ControllerBases;
using UdemyMicroservices.Shared.Services;

namespace UdemyMicroservices.Basket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : CustomBaseController
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public BasketController(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService ?? throw new ArgumentNullException(nameof(basketService));
            _sharedIdentityService = sharedIdentityService ?? throw new ArgumentNullException(nameof(sharedIdentityService));
        }

        [HttpGet]
        public async Task<IActionResult> GetBasket()
        {
            //var claims = HttpContext.User.Claims;
            //var claims = User.Claims;

            return CreateActionResultInstance(await _basketService.GetBasket(_sharedIdentityService.GetUserId));
        }

        [HttpPost]
        public async Task<IActionResult> SaveOrUpdate(BasketDto basketDto)
        {
            basketDto.UserId = _sharedIdentityService.GetUserId;

            var response = await _basketService.SaveOrUpdate(basketDto);

            return CreateActionResultInstance(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBasket()
        {
            return CreateActionResultInstance(await _basketService.Delete(_sharedIdentityService.GetUserId));
        }
    }
}
