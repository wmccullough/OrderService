using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using StatelessOrderService.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace OrderServiceWebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            try {
                //NOTES: In the MSDN example, it provides a partition id of '0', don't do this for stateless!!
                //service fabric will resolve the partition itself for stateless!
                ICounter counter =
                ServiceProxy.Create<ICounter>(new Uri("fabric:/OrderService/StatelessOrderService"));

                long count = await counter.GetCountAsync();

                return new string[] { count.ToString() };
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }
    }
}
