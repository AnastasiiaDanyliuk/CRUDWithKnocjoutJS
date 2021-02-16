using System.Web.Http;
using WebWithKnocKoutJS.Services.Interfaces;

namespace WebWithKnocKoutJS.Controllers
{
    public class AddressesController : ApiController
    {
        private readonly IAddressService addressService;

        public AddressesController(IAddressService addressService)
        {
            this.addressService = addressService;
        }

        [HttpGet]
        [Route("api/Addresses/cities")]
        public IHttpActionResult GetCities()
        {
            return Ok(addressService.GetCities());
        }

        [HttpGet]
        [Route("api/Addresses/streets")]
        public IHttpActionResult GetStreets()
        {
            return Ok(addressService.GetStreets());
        }
    }
}
