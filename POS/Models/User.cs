using POS.Models.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Models
{
    [Table("User")]
    public class User : ModelBase
    {
        public string Email { get; set; }

        [NotMapped]
        public string Password { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string PhoneNumber { get; set; }

        public Role RoleId { get; set; }
    }
}
