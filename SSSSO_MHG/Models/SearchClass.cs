using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSSSO_MHG.Models
{
    public class SearchClass
    {
        public int ID { get; set; }

        public string Keyword { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public string Title { get; set; }
        public string URL { get; set; }
        //exclusively for eventSearch
        public DateTime StartDate { get; set; }
    }
}