using System.Web.Http;
using WebWithKnocKoutJS.Services.Interfaces;

namespace WebWithKnocKoutJS.Controllers
{
    public class CountriesController : ApiController
    {
        private readonly ICountryService countryService;

        public CountriesController(ICountryService countryService)
        {
            this.countryService = countryService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(countryService.Get());
        }
    }
}
