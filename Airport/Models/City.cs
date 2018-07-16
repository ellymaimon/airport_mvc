using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

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

        public override bool Equals(System.Object otherCity)
        {
            if (!(otherCity is City))
            {
                return false;
            }
            else
            {
                City newCity = (City)otherCity;
                bool idEquality = Id == newCity.Id;
                bool nameEquality = Name == newCity.Name;
                bool stateEquality = State == newCity.State;
                return (idEquality && nameEquality && stateEquality);
            }
        }


        public static List<City> GetAll()
        {
            List<City> allCities = new List<City> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                string name = rdr.GetString(1);
                string state = rdr.GetString(2);


                City newCity = new City(name, state, id);
                allCities.Add(newCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allCities;
        }

        public static City Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM cities WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int cityId = 0;
            string name = "";
            string state = "";
            // We remove the line setting a itemCategoryId value here.

            while (rdr.Read())
            {
                cityId = rdr.GetInt32(0);
                name = rdr.GetString(1);
                state = rdr.GetString(2);

            }


            City newCity = new City (name, state, cityId);
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newCity;
        }


        public void AddFlight(Flight newFlight)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights_cities (city_id, flight_id) VALUES (@FlightId, @FlightId);";

            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@FlightId";
            city_id.Value = newFlight.Id;
            cmd.Parameters.Add(city_id);

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@FlightId";
            flight_id.Value = Id;
            cmd.Parameters.Add(flight_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public List<Flight> GetFlights()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT flights.* FROM cities
            JOIN flights_cities ON (flights.id = flights_cities.flight_id)
            JOIN cities ON (flights_cities.city_id = cities.id)
            WHERE flights.id = @CategoryId;";

            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = Id;
            cmd.Parameters.Add(flightIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<Flight> flights = new List<Flight> { };

            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int code = rdr.GetInt32(1);
                string adl = rdr.GetString(2);
                TimeSpan time = rdr.GetTimeSpan(3);
                string status = rdr.GetString(4);
                Flight newFlight = new Flight(code, adl, time, status, id);
                flights.Add(newFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return flights;
        }
    }
}











