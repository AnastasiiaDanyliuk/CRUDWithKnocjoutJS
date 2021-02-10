namespace WebWithKnocKoutJS.Models
{ 
    public class CustomerPerCountry
    {
        private int customersCount;
        private string country;

        public int CustomersCount { get => customersCount; set => customersCount = value; }

        public string Country { get => country; set => country = value; }
    }
}