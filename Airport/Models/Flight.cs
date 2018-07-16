using System;
using MySql.Data.MySqlClient;
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
  


        public override bool Equals(System.Object otherFlight)
        {
            if (!(otherFlight is Flight))
            {
                return false;
            }
            else
            {
                Flight newFlight = (Flight) otherFlight;
                bool idEquality = Id == newFlight.Id;
                bool codeEquality = Code == newFlight.Code;
                bool ADLEquality = ADL == newFlight.ADL;
                bool timeEquality = Time == newFlight.Time;
                bool statusEquality = Status == newFlight.Status;
                return (idEquality && codeEquality && ADLEquality && timeEquality && statusEquality);
            }
        }


        public void Save()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();

            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO flights (code, a_d_l, time, status) VALUES (@code, @adl, @time, @status);";


            cmd.Parameters.AddWithValue("@code", this.Code);
            cmd.Parameters.AddWithValue("@adl", this.ADL);
            cmd.Parameters.AddWithValue("@time", this.Time);
            cmd.Parameters.AddWithValue("@status", this.Status);


            cmd.ExecuteNonQuery();
            Id = (int)cmd.LastInsertedId;
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static List<Flight> GetAll()
        {
            List<Flight> allFlights = new List<Flight> { };
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while (rdr.Read())
            {
                int id = rdr.GetInt32(0);
                int code = rdr.GetInt32(1);
                string adl = rdr.GetString(2);
                DateTime time = rdr.GetDateTime(3);
                string status = rdr.GetString(4);

                Flight newFlight = new Flight(code, adl, time, status, id);
                allFlights.Add(newFlight);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allFlights;
        }

        public static Flight Find(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM flights WHERE id = (@searchId);";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = id;
            cmd.Parameters.Add(searchId);

            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            int _id = 0;
            int code = 0;
            DateTime time = DateTime.MinValue;
            string adl = "";
            string status = "";

            while (rdr.Read())
            {
                _id = rdr.GetInt32(0);
                code = rdr.GetInt32(1);
                adl = rdr.GetString(2);
                time = rdr.GetDateTime(3);
                status = rdr.GetString(4);
            }

            Flight newFlight = new Flight(code, adl, time, status, id);

            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }

            return newFlight;
        }


        public void Delete()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights WHERE id = @searchId; DELETE FROM flights_cities WHERE id = @searchId";

            MySqlParameter searchId = new MySqlParameter();
            searchId.ParameterName = "@searchId";
            searchId.Value = Id;
            cmd.Parameters.Add(searchId);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM flights; DELETE FROM flights_cities;";
            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public void AddCity(City newCity)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO cities_flights (city_id, flight_id) VALUES (@CityId, @FlightId);";

            MySqlParameter city_id = new MySqlParameter();
            city_id.ParameterName = "@CityId";
            city_id.Value = newCity.Id;
            cmd.Parameters.Add(city_id);

            MySqlParameter flight_id = new MySqlParameter();
            flight_id.ParameterName = "@CityId";
            flight_id.Value = Id;
            cmd.Parameters.Add(flight_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }


        public List<City> GetCities()
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT cities.* FROM flights
            JOIN flights_cities ON (flights.id = flights_cities.flight_id)
            JOIN cities ON (flights_cities.city_id = cities.id)
            WHERE flights.id = @CategoryId;";

            MySqlParameter flightIdParameter = new MySqlParameter();
            flightIdParameter.ParameterName = "@FlightId";
            flightIdParameter.Value = Id;
            cmd.Parameters.Add(flightIdParameter);

            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            List<City> cities = new List<City> { };

            while (rdr.Read())
            {
                int cityId = rdr.GetInt32(0);
                string cityName = rdr.GetString(1);
                string cityState = rdr.GetString(2);
                City newCity = new City(cityName, cityState, cityId);
                cities.Add(newCity);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return cities;
        }
    }
}