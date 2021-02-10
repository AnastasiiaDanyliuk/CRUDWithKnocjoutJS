using System.Web.Http;
using WebWithKnocKoutJS.Models;
using WebWithKnocKoutJS.Services.Interfaces;

namespace WebWithKnocKoutJS.Controllers
{
    public class CustomersController : ApiController
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(customerService.Get());
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var cus = customerService.Get(id);
            if (cus == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cus);
            }
        }

        [HttpPost]
        public IHttpActionResult Add(Customer customer)
        {
            return Ok(customerService.Add(customer));
        }

        [HttpPut]
        public IHttpActionResult Update(Customer customer)
        {
            var cus = customerService.Get(customer.CustID);
            if (cus == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customerService.Update(cus, customer));
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var customer = customerService.Get(id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customerService.Delete(customer));
            }
        }

        [HttpGet]
        [Route("api/Customers/chart")]
        public IHttpActionResult GetChart()
        {
            return Ok(customerService.GetChart());
        }
    }
}
