using CoffeeMachineWebAPI.Models;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;

namespace CoffeeMachineWebAPI.Controllers
{
    public class QueueController : ApiController
    {
        private CoffeeContext context;

        private CoffeeContext Context
        {
            get
            {
                if (context == null)
                {
                    context = new CoffeeContext();
                }

                return context;
            }
        }

        [HttpGet]
        [Route("api/Queue")]
        public JsonResult<short> GetLastFromQueue()

        {
            var ret = new Order();
            var queueDao = new QueueModel(Context);
            var queue = queueDao.GetQueue();

            if (queue.Count > 0)
            {
                ret = queue.Dequeue();
                ret.ProcessingDate = DateTime.Now;

                Context.UpdateOrder(ret);
            }

            return Json(ret.OrderType ?? default(short));
        }

        [HttpGet]
        [Route("api/Queue/all")]
        public JsonResult<short[]> WholeQueue()
        {
            var queueDao = new QueueModel(Context);

            return Json(Context.GetAll()
                .Select(o => o.OrderType.Value)
                .ToArray());
        }

        [HttpPost]
        [Route("api/Queue/add")]
        public void AddTaskToQueue(short code)
        {
            Context.InsertOrder(new Order()
            {
                OrderType = code
            });
        }
    }
}