using OrderActor.Interfaces;
using Microsoft.ServiceFabric.Actors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using StatelessOrderService.Data;
using System.Data.Entity;
using Microsoft.ServiceFabric.Data.Collections;

namespace OrderActor
{
    /// <remarks>
    /// Each ActorID maps to an instance of this class.
    /// The IProjName  interface (in a separate DLL that client code can
    /// reference) defines the operations exposed by ProjName objects.
    /// </remarks>
    internal class OrderActor : StatefulActor<OrderActor.ActorState>, IOrderActor
    {
        /// <summary>
        /// This class contains each actor's replicated state.
        /// Each instance of this class is serialized and replicated every time an actor's state is saved.
        /// For more information, see http://aka.ms/servicefabricactorsstateserialization
        /// </summary>
        [DataContract]
        internal sealed class ActorState
        {

            [DataMember]
            public Dictionary<Guid, Order> Orders { get; set; }
        }

        /// <summary>
        /// This method is called whenever an actor is activated.
        /// </summary>
        protected override Task OnActivateAsync()
        {
            if (this.State == null) {
                // This is the first time this actor has ever been activated.
                // Set the actor's initial state values.
                this.State = new ActorState { Orders = new Dictionary<Guid, Order>() };
            }

            ActorEventSource.Current.ActorMessage(this, "State initialized to {0}", this.State);
            return Task.FromResult(true);
        }

        [Readonly]
        public async Task<Order> Get(Guid orderKey)
        {
            if (!this.State.Orders.ContainsKey(orderKey)) {
                ActorEventSource.Current.ActorMessage(this, "Readthrough :(");
                using (OrdersContext db = new OrdersContext()) {
                    db.Configuration.ProxyCreationEnabled = false;

                    Order order = await db.Orders.FirstOrDefaultAsync(o => o.OrderKey == orderKey);
                    this.State.Orders[orderKey] = order;
                    return order;
                }
            } else {
                ActorEventSource.Current.ActorMessage(this, "Saved round trip!, used state instead!");
                return this.State.Orders[orderKey];
            }
            
        }

        [Readonly]
        public async Task<IEnumerable<Order>> GetAll()
        {
            using (OrdersContext db = new OrdersContext()) {
                db.Configuration.ProxyCreationEnabled = false;

                List<Order> orders = await db.Orders.ToListAsync<Order>();
                return orders;
            }
        }

    }
}
