using System;
using System.Linq;
using System.Collections.Generic;

namespace Airport.Models
                 
{
    public class Flight
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string ADL { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }



        public Flight(int code, string adl, DateTime time, string status, int id = 0)
        {
            Code = code;
            ADL = adl;
            Time = time;
            Status = status;
            Id = id;

        }
  


        }

}
