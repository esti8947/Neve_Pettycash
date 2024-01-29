using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models_Dto
{
    public class BuyerDto
    {

        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public string BuyerNameHeb { get; set; }
    }
}
