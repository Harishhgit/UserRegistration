using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registrationform.Models;

namespace Registrationform.Services
{
    public class Shredservice : ISharedservices
    {
        private readonly UserContext _usercontext;

        public Shredservice(UserContext usercontext) 
        {
            _usercontext = usercontext;
        }
        public List<Country> GetCountry() 
        {
            var countries = _usercontext.Countries.ToList();
            return countries;
        }

        public List<State> GetState(int countryid) 
        {
            var states = _usercontext.States.Where(s => s.CountryId == countryid).ToList();
            return states;
        }

        public List<City> GetCity(int stateid)
        {
            var cities = _usercontext.City.Where(c => c.StateId == stateid).ToList();
            return cities;
        }
    }
}
