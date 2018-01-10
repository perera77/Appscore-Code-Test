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

            try
            {
                IEnumerable<Person> people = DataStore.Instance.People;

                IEnumerable<Person> selected = (from p in people
                            where p.name.ToUpper().Contains(searchKey) && (string.IsNullOrEmpty(genderKey) || p.gender == genderKey)
                            select p);

                if (selected.Count() > 0)
                {
                    var page = selected.Skip(10 * (pageRequired - 1)).Take(10); // only returns the first ten entries

                    return Ok(new
                    {
                        people = page.Select(p => new { id = p.id, name = p.name, gender = p.gender == "M" ? "Male" : "Female", birthplace = place_of_birth(p.place_id) }),
                        page = pageRequired,
                        total_pages = (selected.Count() + 9) / 10
                    });
                }
                else
                    return NotFound();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        // GET: api/ancestry/search
        [HttpGet]
        [Route("api/ancestry/advancedsearch")]
        public IHttpActionResult advancedSearch(string key, bool male = false, bool female = false, bool directionAncestors=true)
        {
            if (string.IsNullOrEmpty(key))
                return NotFound();

            string genderKey = "";
            if (male && !female)
                genderKey = "M";
            else if (!male && female)
                genderKey = "F";

            string searchKey = key.ToUpper();
            List<Person> selected = new List<Person>();
            try
            {
                IEnumerable<Person> people = DataStore.Instance.People;
                Person searchPerson = people.FirstOrDefault(p => p.name.Equals(searchKey, StringComparison.CurrentCultureIgnoreCase) &&
                    (string.IsNullOrEmpty(genderKey) || p.gender == genderKey));
                
                if (searchPerson !=  null)
                {
                    if(directionAncestors)
                    {
                        List<Person> currentSerchList = new List<Person>();
                        currentSerchList.Add(searchPerson);
                        

                        while (selected.Count < 10)
                        {
                            List<Person> nextSerchList = new List<Person>();
                            foreach (Person sp in currentSerchList)
                            {
                                if (sp.father_id != null)
                                {
                                    Person father = people.FirstOrDefault(p => p.id == sp.father_id);
                                    if (father != null)
                                    {
                                        selected.Add(father);
                                        nextSerchList.Add(father);
                                    }
                                }

                                if (selected.Count > 9)
                                    break;

                                if (sp.mother_id != null)
                                {
                                    Person mother = people.FirstOrDefault(p => p.id == sp.mother_id);
                                    if (mother != null)
                                    {
                                        selected.Add(mother);
                                        nextSerchList.Add(mother);
                                    }
                                }
                                if (selected.Count > 9)
                                    break;
                            }

                            currentSerchList = nextSerchList;
                        }
                    }
                    else // Descendent search
                    {

                    }

                    return Ok(selected.Select(p => new { id = p.id, name = p.name, gender = p.gender == "M" ? "Male" : "Female", birthplace = place_of_birth(p.place_id) }));
                }
                else
                    return NotFound();
            }
            catch (Exception)
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
