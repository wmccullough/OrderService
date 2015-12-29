using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using StatelessOrderService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StatelessOrderService.Data;
using System.Data.Entity;

namespace StatelessOrderService
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class StatelessOrderService : StatelessService, IOrderRepository
    {
        public async Task<Order> Get(Guid orderKey)
        {
            using (OrdersContext db = new OrdersContext()) {
                db.Configuration.ProxyCreationEnabled = false;

                Order order = await db.Orders.FirstOrDefaultAsync(o => o.OrderKey == orderKey);
                return order;
            }
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            using (OrdersContext db = new OrdersContext()) {
                db.Configuration.ProxyCreationEnabled = false;

                List<Order> orders = await db.Orders.ToListAsync<Order>();
                return orders;
            }
        }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new List<ServiceInstanceListener>()
            {
                new ServiceInstanceListener(
                    (initParams) =>
                        new ServiceRemotingListener<IOrderRepository>(initParams, this))
            };
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancelServiceInstance">Canceled when Service Fabric terminates this instance.</param>
        protected override async Task RunAsync(CancellationToken cancelServiceInstance)
        {
            // TODO: Replace the following sample code with your own logic.

            int iterations = 0;
            // This service instance continues processing until the instance is terminated.
            while (!cancelServiceInstance.IsCancellationRequested) {

                // Log what the service is doing
                ServiceEventSource.Current.ServiceMessage(this, "Working-{0}", iterations++);

                // Pause for 1 second before continue processing.
                await Task.Delay(TimeSpan.FromSeconds(1), cancelServiceInstance);
            }
        }
    }
}
