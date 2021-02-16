using System.Collections.Generic;
using System.Linq;
using WebWithKnocKoutJS.Services.Interfaces;

namespace WebWithKnocKoutJS.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly DBCustomerEntities db;

        public AddressService(DBCustomerEntities dBCustomerEntities)
        {
            db = dBCustomerEntities;
        }

        public Address Add(Address address)
        {
            var newAddress = db.Addresses.Add(new Address
            {
                Country = address.Country,
                CountryRef = address.CountryRef,
                City = address.City,
                Street = address.Street,
            });
            db.SaveChanges();

            return newAddress;
        }

        public Address Get(int id)
        {
            return db.Addresses.Where(addr => addr.AddressID == id).FirstOrDefault();
        }

        public Address Get(Address address, int countryId)
        {
            return db.Addresses.Where(addr => addr.CountryRef == countryId &&
                addr.City == address.City && addr.Street == address.Street)
                .FirstOrDefault();
        }

        public void Remove(Address address)
        {
            db.Addresses.Remove(address);
            db.SaveChanges();
        } 

        public List<string> GetCities()
        {
            return db.Addresses.Select(addr=>addr.City).Distinct().ToList();
        }

        public List<string> GetStreets()
        {
            return db.Addresses.Select(addr => addr.Street).Distinct().ToList();
        }
    }
}