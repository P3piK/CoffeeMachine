using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace CoffeeMachineWebAPI.Models
{
    public class QueueModel
    {
        private CoffeeContext _context;

        public QueueModel(CoffeeContext context)
        {
            _context = context;
        }

        public enum QueueCode
        {
            Empty,
            TurnOn,
            TurnOff
        }

        public Queue<Order> OrderQueue { get; set; }
        
        public Queue<Order> GetQueue()
        {
            return new Queue<Order>(_context.GetNotProcessed());
        }
    }
}
