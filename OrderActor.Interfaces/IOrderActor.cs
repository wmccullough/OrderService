using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using StatelessOrderService.Data;

namespace OrderActor.Interfaces
{
    /// <summary>
    /// This interface represents the actions a client app can perform on an actor.
    /// It MUST derive from IActor and all methods MUST return a Task.
    /// </summary>
    public interface IOrderActor : IActor
    {
        Task<Order> Get(Guid orderKey);
        Task<IEnumerable<Order>> GetAll();
    }
}
