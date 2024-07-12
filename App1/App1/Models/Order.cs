using System.ComponentModel.DataAnnotations;

namespace App1.Models
{
    public class Order
    {
        [Key] 
        public int Id { get; set; } 
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string UserId {get;set;}
    }
}
