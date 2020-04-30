using EventLite.Domain.EventLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventCatalogApi.ViewModels
{
    public class TicketTypesViewModel
    {
        public int CatalogEventId { get; set; }

        public IEnumerable<CatalogTicketType> CatalogTicketTypes { get; set; }

    }
}
