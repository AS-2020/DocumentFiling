using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Dokumentenablage.Models
{
    class StorageLocation
    {
        public int LocationID { get; set; }
        private string floor;
        public string Floor
        {
            get { return floor; }
            set { floor = value; }
        }

        private string roomNumber;

        public string RoomNumber
        {
            get { return roomNumber; }
            set { roomNumber = value; }
        }
        private string shelfNumber;

        public string ShelfNumber
        {
            get { return shelfNumber; }
            set { shelfNumber = value; }
        }

        private string shelf;

        public string Shelf
        {
            get { return shelf; }
            set { shelf = value; }
        }

        public override string ToString()
        {
            return $"Floor: {Floor}, Room number: {RoomNumber}, Shelf number: {ShelfNumber}, Shelf: {Shelf}";
        }
    }
    class StorageLocationHandler
    {
        private List<StorageLocation> locations;

        public StorageLocationHandler()
        {
            locations = new List<StorageLocation>();
        }
        private static StorageLocationHandler instance;
        public static StorageLocationHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StorageLocationHandler();
                }
                return instance;
            }
        }
        public void AddStorageLocation(StorageLocation storageLocation)
        {
            locations.Add(storageLocation);
        }

        public List<StorageLocation> GetStorageLocations()
        {
            return locations;
        }
        public void RemoveStorageLocation(StorageLocation storageLocation)
        {
            locations.Remove(storageLocation);
        }


        private const string VERBINDUNG = @"Data Source=localhost\sqlexpress;Initial Catalog=DocumentFiling;User ID=sa;Password=P@ssword";

        public void Save(StorageLocation storageLocation)
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            con.Open();
            SqlCommand com = new SqlCommand();

            string sql = $"Insert into storageLocation values ({storageLocation.Floor}, '{storageLocation.RoomNumber}', '{storageLocation.ShelfNumber}', " +
                $"'{storageLocation.Shelf   }')";
            com = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.InsertCommand = com;
            adapter.InsertCommand.ExecuteNonQuery();

            com.Dispose();
            con.Close();
        }

        public void Load()
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            string sql = "Select * from storageLocation";

            con.Open();

            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                StorageLocation storageLocation = new StorageLocation();

                storageLocation.LocationID = (int)reader.GetValue(0);
                storageLocation.Floor = reader.GetValue(1).ToString();
                storageLocation.RoomNumber = reader.GetValue(2).ToString();
                storageLocation.ShelfNumber = reader.GetValue(3).ToString();
                storageLocation.Shelf = reader.GetValue(4).ToString();
                Instance.AddStorageLocation(storageLocation);
            }

            reader.Close();
            com.Dispose();
            con.Close();
        }

        public void Change(StorageLocation r, bool[] geandert)
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            con.Open();

            string sql = $@"Update storageLocation set {(geandert[0] ? $"BuildingFloor = '{r.Floor}'" : "")}{((geandert[0] && geandert[1] | geandert[2] | geandert[3]) ? "," : "")}" +
                $"{(geandert[1] ? $"RoomNumber = '{r.RoomNumber}'" : "")}{((geandert[1] && geandert[2] | geandert[3]) ? "," : "")}" +
                $"{ (geandert[2] ? $"ShelfNumber = '{r.ShelfNumber}'" : "")}{((geandert[2] && geandert[3]) ? "," : "")}" +
                $"{ (geandert[3] ? $"Shelf = '{r.Shelf}'" : "")} where LocationID = '{r.LocationID}'";
            SqlCommand com = new SqlCommand(sql, con);
            SqlDataAdapter adapter = new SqlDataAdapter();

            adapter.UpdateCommand = com;
            adapter.UpdateCommand.ExecuteNonQuery();

            com.Dispose();
            con.Close();
        }
    }
}