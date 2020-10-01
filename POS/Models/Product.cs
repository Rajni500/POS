using System.ComponentModel.DataAnnotations.Schema;

namespace POS.Models
{
    [Table("Product")]
    public class Product : ModelBase
    {
        public int UnitPrice { get; set; }

        public int AvailableQuantity { get; set; }

        public string ImageName { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
