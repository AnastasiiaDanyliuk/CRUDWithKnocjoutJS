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
            return db.Countries.Distinct().ToList();
        }

        public Country Get(Country country)
        {
            return db.Countries.Where(_country => _country.CountryName ==
                country.CountryName).FirstOrDefault();
        }

        public Country Add(Country country)
        {
            var newCountry = db.Countries.Add(new Country { CountryName = country.CountryName });
            db.SaveChanges();

            return newCountry;
        }
    }
}