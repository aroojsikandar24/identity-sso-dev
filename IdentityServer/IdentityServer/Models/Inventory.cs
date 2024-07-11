using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Models
{
    public class Inventory
    {
        [Key] // Define this property as the primary key
        public int Id { get; set; } // Example assuming integer primary key
        public string ProductName { get; set; }
        public int QuantityInWarehouse { get; set; }
    }
}
