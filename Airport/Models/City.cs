using System;


namespace Airport.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }


        public City(string name, string state, int id = 0)
        {          
            Name = name;
            State = state;
            Id = id;
        }





















    }
}
