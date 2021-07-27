using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyMicroservices.Order.Infrastructure;
using UdemyMicroservices.Shared.Messages.Commands;

namespace UdemyMicroservices.Order.Application.Consumers
{
    public class CreateOrderMessageCommandConsumer : IConsumer<CreateOrderMessageCommand>
    {
        private readonly OrderDbContext _orderDbContext;

        public CreateOrderMessageCommandConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext ?? throw new ArgumentNullException(nameof(orderDbContext));
        }

        public async Task Consume(ConsumeContext<CreateOrderMessageCommand> context)
        {
            var address = new Domain.OrderAggregate.Address(context.Message.Address.Province,
                                                               context.Message.Address.District,
                                                               context.Message.Address.Street,
                                                               context.Message.Address.ZipCode,
                                                               context.Message.Address.Line);

            Domain.OrderAggregate.Order order = new Domain.OrderAggregate.Order(context.Message.BuyerId, address);

            context.Message.OrderItems.ForEach(x =>
            {
                order.AddOrderItem(x.ProductId, x.ProductName, x.PictureUrl, x.Price);
            });

            await _orderDbContext.Orders.AddAsync(order);

            await _orderDbContext.SaveChangesAsync();
        }
    }
}
