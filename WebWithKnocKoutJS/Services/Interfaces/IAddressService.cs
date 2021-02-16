using System.Collections.Generic;

namespace WebWithKnocKoutJS.Services.Interfaces
{
    public interface IAddressService
    {
        List<string> GetCities();

        List<string> GetStreets();

        Address Get(int id);

        Address Get(Address address, int countryId);

        Address Add(Address address);

        void Remove(Address address);
    }
}
