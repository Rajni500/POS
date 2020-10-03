using Microsoft.EntityFrameworkCore;
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

        public void UpdateProductQuantities(Invoice entity)
        {
            if (entity.BillItems != null)
            {
                foreach (var item in entity.BillItems)
                {
                    var product = Context.Product.Where(i => i.Id == item.ProductId).SingleOrDefault();
                    if (product != null)
                    {
                        product.AvailableQuantity -= item.Quantity;
                        Context.Product.Attach(product);
                        Context.Entry(entity).State = EntityState.Modified;
                        Save();
                        Context.Entry(entity).State = EntityState.Detached;
                    }
                }
            }
        }
    }
}
