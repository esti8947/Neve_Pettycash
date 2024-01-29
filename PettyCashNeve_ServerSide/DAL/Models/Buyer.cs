using System;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class Buyer
    {
        [Key]
        public int BuyerId { get; set; }
        [MaxLength(50)]
        public string BuyerName { get; set; }
        [MaxLength(50)]
        public string BuyerNameHeb { get; set; }
    }
}
