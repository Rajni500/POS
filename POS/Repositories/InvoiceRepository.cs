using POS.Contract;
using POS.Models;
using POS.Repositories.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Repositories
{
    public class InvoiceRepository
        : GenericRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(POSDBContext context)
            : base(context)
        {
        }
    }
}
