using System.Collections.Generic;
using WebWithKnocKoutJS.Models;

namespace WebWithKnocKoutJS.Services.Interfaces
{
    public interface ICustomerService
    {
        List<Customer> Get();

        Customer Get(int id);

        Customer Add(Customer customer);

        bool Delete(Customer customer);

        Customer Update(Customer customer, Customer newCustomer);

        List<CustomerPerCountry> GetChart();
    }
}