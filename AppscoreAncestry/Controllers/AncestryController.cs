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

            IEnumerable<Person> selected;
            try
            {
                IEnumerable<Person> people = DataStore.Instance.People;
                if (string.IsNullOrEmpty(genderKey))
                {
                    selected = (from p in people
                                where p.name.Contains(key)
                                select p);
                }
                else
                {
                    selected = (from p in people
                                where p.name.Contains(key) && p.gender == genderKey
                                select p);
                }
                if(selected.Count() > 0)
                    return Ok(selected.Take(10)); // only returns the first ten entries - TODO implement paging
                else
                    return NotFound();
            }
            catch(Exception)
            {
                return NotFound();
            }
            
        }
    }
}
