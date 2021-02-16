using System.Collections.Generic;
using System.Linq;
using WebWithKnocKoutJS.Services.Interfaces;

namespace WebWithKnocKoutJS.Services.Implementations
{
    public class CountryService : ICountryService
    {
        private readonly DBCustomerEntities db;

        public CountryService(DBCustomerEntities dBCustomerEntities)
        {
            db = dBCustomerEntities;
        }

        public List<Country> Get()
        {
            return db.Countries.ToList();
        }
    }
}