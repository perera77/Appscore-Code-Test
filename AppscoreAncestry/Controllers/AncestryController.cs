using AppscoreAncestry.DataLayer;
using AppscoreAncestry.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppscoreAncestry.Controllers
{
    [Route("api/ancestry")]
    public class AncestryController : ApiController
    {
        // GET: api/ancestry/search
        [HttpGet]
        [Route("api/ancestry/search")]
        public IHttpActionResult searchPeople(string key, bool male = false, bool female = false, int pageRequired = 1)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();

            string genderKey = "";
            if (male && !female)
                genderKey = "M";
            else if (!male && female)
                genderKey = "F";

            string searchKey = key.ToUpper();

            IEnumerable<Person> selected;
            try
            {
                IEnumerable<Person> people = DataStore.Instance.People;
                if (string.IsNullOrEmpty(genderKey))
                {
                    selected = (from p in people
                                where p.name.ToUpper().Contains(searchKey)
                                select p);
                }
                else
                {
                    selected = (from p in people
                                where p.name.ToUpper().Contains(searchKey) && p.gender == genderKey
                                select p);
                }
                if (selected.Count() > 0)
                {
                    var page = selected.Skip(10 * (pageRequired - 1)).Take(10); // only returns the first ten entries - TODO implement paging

                    return Ok(new {
                        people = page.Select(p => new { id = p.id, name = p.name, gender = p.gender == "M" ? "Male" : "Female", birthplace = place_of_birth(p.place_id) }),
                        page = pageRequired,
                        total_pages = (selected.Count() + 9) / 10
                    });
                }
                else
                    return NotFound();
            }
            catch(Exception)
            {
                return NotFound();
            }
            
        }

        private string place_of_birth(int id)
        {
            IEnumerable <Place> places = DataStore.Instance.Places;
            var select = places.FirstOrDefault(place => place.id == id);
            return select != null ? select.name : "";
        }
    }
}
