using Registrationform.Models;

namespace Registrationform.Services
{
    public interface ISharedservices
    {
        List<Country> GetCountry();
        List<State> GetState(int countryid);
        List<City> GetCity(int stateid);
    }
}
