using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Dokumentenablage.Models
{
    class ResponsiblePerson
    {
        public int PersonID { get; set; }
        public string Name { get; set; }
        public string OfficeNumber { get; set; }

        public string Department { get; set; }
        public override string ToString()
        {
            return $"Name: {Name} Office: {OfficeNumber}";
        }
    }
    class ResponsiblePersonHandler
    {
        private List<ResponsiblePerson> persons;

        public ResponsiblePersonHandler()
        {
            persons = new List<ResponsiblePerson>();
        }
        private static ResponsiblePersonHandler instance;
        public static ResponsiblePersonHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ResponsiblePersonHandler();
                }
                return instance;
            }
        }
        public void AddResponsiblePerson(ResponsiblePerson responsiblePerson)
        {
            persons.Add(responsiblePerson);
        }

        public List<ResponsiblePerson> GetResponsiblePerson()
        {
            return persons;
        }
        public void RemoveStorageLocation(ResponsiblePerson responsiblePerson)
        {
            persons.Remove(responsiblePerson);
        }


        private const string VERBINDUNG = @"Data Source=localhost\sqlexpress;Initial Catalog=DocumentFiling;User ID=sa;Password=P@ssword";

        public void Save(ResponsiblePerson responsiblePerson)
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            con.Open();
            SqlCommand com = new SqlCommand();

            string sql = $"Insert into responsiblePerson values ({responsiblePerson.Name}, '{responsiblePerson.OfficeNumber}', '{responsiblePerson.Department}')";
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

            string sql = "Select * from responsiblePerson";

            con.Open();

            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                ResponsiblePerson responsiblePerson = new ResponsiblePerson();

                responsiblePerson.PersonID = (int)reader.GetValue(0);
                responsiblePerson.Name = reader.GetValue(1).ToString();
                responsiblePerson.OfficeNumber = reader.GetValue(2).ToString();
                responsiblePerson.Department = reader.GetValue(3).ToString();
                Instance.AddResponsiblePerson(responsiblePerson);
            }

            reader.Close();
            com.Dispose();
            con.Close();
        }

        //public void Change(ResponsiblePerson responsiblePerson, bool[] geandert)
        //{
        //    SqlConnection con = new SqlConnection(VERBINDUNG);

        //    con.Open();

        //    string sql = $@"Update responsiblePerson set {(geandert[0] ? $"DatumFaelligkeit = '{r.DatumFaelligkeit}'" : "")}{((geandert[0] && geandert[1] | geandert[2] | geandert[3]) ? "," : "")}" +
        //        $"{(geandert[1] ? $"Kundennummer = '{r.KundenNummer}'" : "")}{((geandert[1] && geandert[2] | geandert[3]) ? "," : "")}" +
        //        $"{ (geandert[2] ? $"Summe = '{summemitpunkt}'" : "")}{((geandert[2] && geandert[3]) ? "," : "")}" +
        //        $"{ (geandert[3] ? $"DatumBegleichung = '{r.DatumBegleichung}'" : "")} where Rechnungsnummer = '{r.RechnungsNummer}'";
        //    SqlCommand com = new SqlCommand(sql, con);
        //    SqlDataAdapter adapter = new SqlDataAdapter();

        //    adapter.UpdateCommand = com;
        //    adapter.UpdateCommand.ExecuteNonQuery();

        //    com.Dispose();
        //    con.Close();
        //}
    }
}
