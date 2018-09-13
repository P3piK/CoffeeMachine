using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeMachineWebAPI.Models
{
    [Serializable]
    public class Order
    {
        public int? Id { get; set; }
        public short? OrderType { get; set; }
        public DateTime? CreationDate { get; set; }
        public DateTime? ProcessingDate { get; set; }
    }
}
