using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCR.Test.Models
{
    public class DrivingLicenceViewModel
    {
        public int code { get; set; }
        public string message { get; set; }
        public Data data { get; set; }

        public class Data
        {
            public string session_id { get; set; }
            public List<Item> items { get; set; }
        }

        public class Item
        {
            public string item { get; set; }
            public Itemcoord itemcoord { get; set; }
            public float itemconf { get; set; }
            public string itemstring { get; set; }
        }

        public class Itemcoord
        {
            public int x { get; set; }
            public int y { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }
    }

}
