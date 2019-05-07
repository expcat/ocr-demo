using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCR.Test.Models
{
    public class IDCardViewModel
    {
        public List<ResultList> result_list { get; set; }

        public class ResultList
        {
            public int code { get; set; }
            public string message { get; set; }
            public string filename { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public string name { get; set; }
            public string sex { get; set; }
            public string nation { get; set; }
            public string birth { get; set; }
            public string address { get; set; }
            public string id { get; set; }
            public string authority { get; set; }
            public string valid_date { get; set; }
            public int[] name_confidence_all { get; set; }
            public int[] sex_confidence_all { get; set; }
            public int[] nation_confidence_all { get; set; }
            public int[] birth_confidence_all { get; set; }
            public int[] address_confidence_all { get; set; }
            public int[] id_confidence_all { get; set; }
            public int[] authority_confidence_all { get; set; }
            public int[] valid_date_confidence_all { get; set; }
            public int card_type { get; set; }
            public object[] recognize_warn_code { get; set; }
            public object[] recognize_warn_msg { get; set; }
        }
    }

}
