using System.Collections.Generic;
using System.Linq;
using WebWithKnocKoutJS.Services.Interfaces;
using System.Data.Entity;
using WebWithKnocKoutJS.Dto;

namespace WebWithKnocKoutJS.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly DBCustomerEntities db;

        public CustomerService(DBCustomerEntities dBCustomerEntities)
        {
            db = dBCustomerEntities;
        }

        public List<Customer> Get()
        {
            return db.Customers.ToList();
        }

        public Customer Get(int id)
        {
            return db.Customers.Find(id);
        }

        public Customer Add(Customer customer)
        {
            var customerCountry = db.Countries.Where(country => country.CountryName ==
                customer.Address.Country.CountryName).FirstOrDefault();

            var customerAddress = db.Addresses.Where(addr => addr.CountryRef == customerCountry.CountryID &&
                addr.City == customer.Address.City && addr.Street == customer.Address.Street)
                .FirstOrDefault();

            if (customerAddress == null)
            {
                customerAddress = db.Addresses.Add(new Address
                {
                    Country = customerCountry,
                    CountryRef = customerCountry.CountryID,
                    City = customer.Address.City,
                    Street = customer.Address.Street,
                });
            }

            var newCustomer = db.Customers.Add(new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                AddressRef = customerAddress.AddressID,
                Address = customerAddress,
            });

            db.SaveChanges();
            return newCustomer;
        }

        public bool Delete(Customer customer)
        {
            var customerCountry = db.Countries.Where(
                country => country.CountryName == customer.Address.Country.CountryName).FirstOrDefault();
            var address = db.Addresses.Where(addr => addr.CountryRef == customerCountry.CountryID &&
                addr.City == customer.Address.City && addr.Street == customer.Address.Street)
                .FirstOrDefault();
            var deletedCustomer = db.Customers.Remove(customer);

            if (address.Customers.Count() == 0)
            {
                var deletedAddress = db.Addresses.Remove(address);
            }

            db.SaveChanges();
            return deletedCustomer != null;
        }

        public Customer Update(Customer customer, Customer newCustomer)
        {
            var customerCountry = db.Countries.Where(
                  country => country.CountryName == newCustomer.Address.Country.CountryName).FirstOrDefault();

            var oldAddress = db.Addresses.Where(address => address.CountryRef == customer.Address.Country.CountryID &&
                address.City == customer.Address.City && address.Street == customer.Address.Street)
                .FirstOrDefault();

            var newAddress = db.Addresses.Where(address => address.CountryRef == customerCountry.CountryID &&
                address.City == newCustomer.Address.City && address.Street == newCustomer.Address.Street)
                .FirstOrDefault();

            if (newAddress != null)
            {
                customer.Address = newAddress;
                customer.AddressRef = newAddress.AddressID;
                if (oldAddress.Customers.Count() == 1)
                {
                    var address = db.Addresses.Where(addr => addr.AddressID == oldAddress.AddressID).FirstOrDefault();
                    db.Addresses.Remove(address);
                }
            }
            else
            {
                var address = newCustomer.Address;
                address.Country = oldAddress.Country;
                address.CountryRef = oldAddress.CountryRef;

                if (oldAddress.Customers.Count() != 1)
                {
                    address = db.Addresses.Add(new Address
                    {
                        City = newCustomer.Address.City,
                        Street = newCustomer.Address.Street,
                        Country = customerCountry,
                        CountryRef = customerCountry.CountryID,
                    });
                    db.SaveChanges();
                    address = db.Addresses.Where(addr => addr.CountryRef == address.CountryRef &&
                        addr.City == address.City && addr.Street == addr.Street).FirstOrDefault();

                }

                db.Entry(customer.Address).CurrentValues.SetValues(address);
            }
            
            customer.Email = newCustomer.Email;
            customer.FirstName = newCustomer.FirstName;
            customer.LastName = newCustomer.LastName;

            db.Entry(customer).State = EntityState.Modified;
            db.SaveChanges();
            return customer;
        }

        public List<CustomerPerCountry> GetChart()
        {
            IEnumerable<Country> countryList = db.Countries;
            IEnumerable<Customer> customerList = db.Customers;
            List<CustomerPerCountry> customerPerCountries = new List<CustomerPerCountry>();

            foreach (var country in countryList)
            {
                int count = customerList.Where(customer => customer.Address.Country.CountryName == country.CountryName).Count();
                customerPerCountries.Add(new CustomerPerCountry() { CustomersCount = count, Country = country.CountryName });
            }
            return customerPerCountries;
        }
    }
}