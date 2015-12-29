﻿using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatelessOrderService.Interfaces
{
    public interface ICounter : IService
    {
        Task<long> GetCountAsync();
    }
}
