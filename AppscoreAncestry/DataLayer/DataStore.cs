using AppscoreAncestry.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;

namespace AppscoreAncestry.DataLayer
{
    /// Singleton Data access helper
    public class DataStore
    {
        private static readonly DataStore _instance = new DataStore();
        private DataSet dataset;

        // Singleton - prevent external initialization
        private DataStore() { }

        public static DataStore Instance { get { return _instance; } }

        private void loadData()
        {
            string small_file = HttpContext.Current.Server.MapPath(@"~\App_Data\data_small.json");
            string large_file = HttpContext.Current.Server.MapPath(@"~\App_Data\data_large.json");

            try
            {
                // deserialize JSON from file
                string Json = System.IO.File.ReadAllText(small_file);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                ser.MaxJsonLength = int.MaxValue;
                dataset = ser.Deserialize<DataSet>(Json);
            }
            catch(Exception ex)
            {

            }
        }

        public IEnumerable<Person> People
        {
            get
            {
                if (dataset == null)
                    loadData();

                return dataset.people;
            }
        }

        public IEnumerable<Place> Places
        {
            get
            {
                if (dataset == null) loadData();
                return dataset.places;
            }
        }
    }


    /// Private data type for JSON file loading
    class DataSet
    {
        public List<Place> places { get; set; }
        public List<Person> people { get; set; }
    }
}