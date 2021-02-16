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
        private readonly ICountryService countryService;
        private readonly IAddressService addressService;

        public CustomerService(DBCustomerEntities dBCustomerEntities, 
                                ICountryService countryService,
                                IAddressService addressService)
        {
            db = dBCustomerEntities;
            this.countryService = countryService;
            this.addressService = addressService;
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
            var customerCountry = countryService.Get(customer.Address.Country);
            if(customerCountry == null)
            {
                customerCountry = countryService.Add(customer.Address.Country);
            }

            var customerAddress = addressService.Get(customer.Address, customerCountry.CountryID);

            if (customerAddress == null)
            {
                var address = customer.Address;
                address.Country = customerCountry;
                address.CountryRef = customerCountry.CountryID;
                customerAddress = addressService.Add(address);
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
            var customerCountry = countryService.Get(customer.Address.Country);

            var address = addressService.Get(customer.Address, customerCountry.CountryID);

            var deletedCustomer = db.Customers.Remove(customer);

            if (address.Customers.Count() == 0)
            {
                addressService.Remove(address);
            }

            db.SaveChanges();
            return deletedCustomer != null;
        }

        public Customer Update(Customer customer, Customer newCustomer)
        {
            var customerCountry = countryService.Get(newCustomer.Address.Country);
            if (customerCountry == null)
            {
                customerCountry = countryService.Add(newCustomer.Address.Country);
            }

            var oldAddress = customer.Address;

            var newAddress = addressService.Get(newCustomer.Address, customerCountry.CountryID);

            if (newAddress != null)
            {
                customer.Address = newAddress;
                customer.AddressRef = newAddress.AddressID;
                if (oldAddress.Customers.Count() == 1)
                {
                    var address = addressService.Get(oldAddress.AddressID);
                    addressService.Remove(address);
                }
            }
            else
            {
                var address = newCustomer.Address;
                address.Country = customerCountry;
                address.CountryRef = customerCountry.CountryID;

                if (oldAddress.Customers.Count() != 1)
                {
                    address = addressService.Add(address);

                    address = addressService.Get(address, address.CountryRef);
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