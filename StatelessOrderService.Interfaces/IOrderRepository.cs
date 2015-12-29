using Microsoft.ServiceFabric.Services.Remoting;
using StatelessOrderService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatelessOrderService.Interfaces
{
    public interface IOrderRepository : IService
    {
        Task<Order> Get(Guid orderKey);
        Task<IEnumerable<Order>> GetAll();
    }
}
