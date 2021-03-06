﻿using POS.Contract.Core;
using POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Contract
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        void UpdateProductQuantities(Invoice entity);
    }
}
