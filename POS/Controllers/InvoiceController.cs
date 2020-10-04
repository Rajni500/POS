using Microsoft.AspNetCore.Mvc;
using POS.Contract;
using POS.Controllers.Core;
using POS.Models;
using POS.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Controllers
{
    [Route("Invoice/")]
    public class InvoiceController : GenericController<Invoice>
    {
        IInvoiceRepository repository;
        public InvoiceController(IInvoiceRepository repository, IServiceProvider serviceProvider, ICache cache)
            : base(repository, cache)
        {
            base.genericRepository = repository;
            base.serviceProvider = serviceProvider;
            this.repository = repository;
            base.cache = cache;
        }

        [HttpPost]
        [AuthorizeLogin]
        [Route("Add")]
        public override ActionResult Add([FromBody] Invoice entity)
        {
            if(entity == null || entity.BillItems == null || entity.BillItems.Count == 0)
            {
                return ExceptionHandler.throwException("No Bill Items Selcted");
            }
            var currentDate = DateTime.Now;
            entity.DateOfSale = currentDate;
            entity.InvoiceNumber = string.Format("I{0}{1}{2}{3}", currentDate.Day, currentDate.Month, currentDate.Year, currentDate.Millisecond);
            var loggedInUser = GetLoggedInUser();
            entity.UserId = loggedInUser.Id;
            entity.User = null;
            foreach(var item in entity.BillItems)
            {
                item.Invoice = null;
                item.Product = null;
            }
            var addedInvoice = base.Add(entity);
            repository.UpdateProductQuantities(entity);
            return Json(addedInvoice);
        }
    }
}
