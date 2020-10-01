using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Models
{
    [Table("BillItems")]
    public class BillItems : ModelBase
    {
        [ForeignKey("Invoice")]
        public int InvoiceId { get; set; }

        public Invoice Invoice { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
