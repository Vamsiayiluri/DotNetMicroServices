using OrderService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto, string userId);
        Task<List<OrderResponseDto>> GetOrdersByUserAsync(string userId);
        Task<OrderResponseDto?> GetByIdAsync(Guid id);
        Task<bool> CancelOrderAsync(Guid id);
    }
}
