using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using StatelessOrderService.Data;
using StatelessOrderService.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OrderActor.Interfaces;
using Microsoft.ServiceFabric.Actors;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderServiceWebApi.Controllers
{
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            try {
                //NOTES: In the MSDN example, it provides a partition id of '0', don't do this for stateless!!
                //service fabric will resolve the partition itself for stateless!
                IOrderActor orderRepository =
                ActorProxy.Create<IOrderActor>(ActorId.NewId(), new Uri("fabric:/OrderService/OrderActorService"));

                IEnumerable<Order> orders = await orderRepository.GetAll();

                return orders;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        // GET api/values/5
        [HttpGet("{orderKey}")]
        public async Task<Order> Get(Guid orderKey)
        {
            try {
                //NOTES: In the MSDN example, it provides a partition id of '0', don't do this for stateless!!
                //service fabric will resolve the partition itself for stateless!
                IOrderActor orderRepository =
                ActorProxy.Create<IOrderActor>(ActorId.NewId(), new Uri("fabric:/OrderService/OrderActorService"));

                Order order = await orderRepository.Get(orderKey);

                return order;
            }
            catch (Exception ex) {
                throw ex;
            }
        }

    }
}
