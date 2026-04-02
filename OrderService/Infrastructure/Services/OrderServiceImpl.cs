using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Persistence;
using OrderService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Infrastructure.Services
{
    public class OrderServiceImpl : IOrderService
    {
        private readonly OrderDbContext _context;

        public OrderServiceImpl(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto, string userId)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                UserId = Guid.Parse(userId),
                Status = "Created",
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new OrderResponseDto
            {
                Id = order.Id,
                Status = order.Status
            };
        }

        public async Task<List<OrderResponseDto>> GetOrdersByUserAsync(string userId)
        {
            var userGuid = Guid.Parse(userId);

            return await _context.Orders
                .Where(o => o.UserId == userGuid)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    Status = o.Status
                })
                .ToListAsync();
        }

        public async Task<OrderResponseDto?> GetByIdAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null) return null;

            return new OrderResponseDto
            {
                Id = order.Id,
                Status = order.Status
            };
        }

        public async Task<bool> CancelOrderAsync(Guid id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null) return false;

            if (order.Status == "Cancelled")
                return false;

            order.Status = "Cancelled";

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
