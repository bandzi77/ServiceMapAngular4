using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Models.Service_Tnt
{
    public class ServiceTntRepository : IServiceTntRepository
    {
        private ApplicationDbContext context;

        public ServiceTntRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<ServiceTnt> ServicesTnt
        {
            get
            {
                return context.ServiceTnt;
            }
        }

    }
}
