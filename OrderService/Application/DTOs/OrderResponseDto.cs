using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DTOs
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = null!;
    }
}
