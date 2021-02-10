using System.Collections.Generic;
using System.Linq;
using WebWithKnocKoutJS.Models;
using WebWithKnocKoutJS.Services.Interfaces;

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
            var newCustomer = db.Customers.Add(customer);
            db.SaveChanges();
            return newCustomer;
        }

        public bool Delete(Customer customer)
        {
            var deletedCustomer = db.Customers.Remove(customer);
            db.SaveChanges();
            return deletedCustomer != null;
        }

        public Customer Update(Customer customer, Customer newCustomer)
        {
            db.Entry(customer).CurrentValues.SetValues(newCustomer);
            db.SaveChanges();
            return customer;
        }

        public List<CustomerPerCountry> GetChart()
        {
            List<string> countryList = new List<string>() { "Ukraine", "Poland", "Germany", "England"};
            IEnumerable<Customer> customerList = db.Customers;
            List<CustomerPerCountry> customerPerCountries = new List<CustomerPerCountry>();

            foreach (var country in countryList)
            {
                int count = customerList.Where(customer => customer.Country == country).Count();
                customerPerCountries.Add(new CustomerPerCountry() { CustomersCount = count, Country = country });
            }
            return customerPerCountries;
        }
    }
}