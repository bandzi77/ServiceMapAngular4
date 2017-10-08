using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceMap.Models.Service_Tnt
{
    public class DepotRepository: IDepotRepository
    {
        private ApplicationDbContext context;

        public DepotRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<DepotDetails> DepotDetails => context.Location;
    }

}
