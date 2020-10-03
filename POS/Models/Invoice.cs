using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Models
{
    [Table("Invoice")]
    public class Invoice : ModelBase
    {
        public string InvoiceNumber { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        public DateTime DateOfSale { get; set; }

        public float DiscountPercent { get; set; }

        public float VAT { get; set; }

        public float SubTotal { get; set; }

        public float InvoiceTotal { get; set; }

        public IList<BillItems> BillItems { get; set; }
    }
}
