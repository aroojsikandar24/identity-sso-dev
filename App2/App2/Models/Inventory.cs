using System.ComponentModel.DataAnnotations;

namespace App2.Models
{
    public class Inventory
    {
        [Key] 
        public int Id { get; set; } 
        public string ProductName { get; set; }
        public int QuantityInWarehouse { get; set; }
    }
}
