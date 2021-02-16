using System.Collections.Generic;

namespace WebWithKnocKoutJS.Services.Interfaces
{
    public interface ICountryService
    {
        List<Country> Get();

        Country Get(Country country);

        Country Add(Country country);
    }
}