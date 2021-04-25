﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UdemyMicroservices.Order.Application.Commands;
using UdemyMicroservices.Order.Application.Dtos;
using UdemyMicroservices.Order.Domain.OrderAggregate;
using UdemyMicroservices.Order.Infrastructure;
using UdemyMicroservices.Shared.Dtos;

namespace UdemyMicroservices.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDto>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newAddress = new Address(request.Address.Province, 
                                        request.Address.District, 
                                        request.Address.Street, 
                                        request.Address.ZipCode, 
                                        request.Address.Line);

            Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(request.BuyerId, newAddress);

            request.OrderItems.ForEach(x =>
            {
                newOrder.AddOrderItem(x.ProductId, x.ProductName, x.PictureUrl, x.Price);
            });

            _context.Orders.Add(newOrder);

            var result = await _context.SaveChangesAsync();

            return Response<CreatedOrderDto>.Success(new CreatedOrderDto { OrderId = newOrder.Id }, 200);
        }
    }
}
