using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_Dokumentenablage.Models
{
    class Document
    {
        public int DocumentNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public StorageLocation Location { get; set; }
        public ResponsiblePerson Person { get; set; }
        public string BriefDescription { get; set; }

        public string CreationDateString
        {
            get
            {
                return CreationDate.ToString("dd.MM.yyyy");
            }
        }
    }

    class DocumentHandler
    {
        private List<Document> documents;

        public DocumentHandler()
        {
            documents = new List<Document>();
        }
        private static DocumentHandler instance;
        public static DocumentHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DocumentHandler();
                }
                return instance;
            }
        }
        public void AddDocument(Document document)
        {
            documents.Add(document);
        }

        public List<Document> GetDocument()
        {
            return documents;
        }
        public void RemoveDocument(Document document)
        {
            documents.Remove(document);
        }


        private const string VERBINDUNG = @"Data Source=localhost\sqlexpress;Initial Catalog=DocumentFiling;User ID=sa;Password=P@ssword";

        public void Save(Document document)
        {
            SqlConnection con = new SqlConnection(VERBINDUNG);

            con.Open();
            SqlCommand com = new SqlCommand();

            string sql = $"Insert into document values ({document.DocumentNumber}, '{document.CreationDate}', '{document.Location}', " +
                $"'{document.Person}', '{document.BriefDescription}')";  //Location und Person noch ändern
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

            string sql = "Select * from document";

            con.Open();

            SqlCommand com = new SqlCommand(sql, con);
            SqlDataReader reader = com.ExecuteReader();

            while (reader.Read())
            {
                Document document = new Document();

                document.DocumentNumber = (int)reader.GetValue(0);
                document.CreationDate = (DateTime)reader.GetValue(1);
                int locationID = (int)reader.GetValue(2);
                document.Location = StorageLocationHandler.Instance.GetStorageLocations().Find(l => l.LocationID == locationID);
                int personID = (int)reader.GetValue(3);
                document.Person = ResponsiblePersonHandler.Instance.GetResponsiblePerson().Find(p => p.PersonID == personID);
                document.BriefDescription = reader.GetValue(4).ToString();
                Instance.AddDocument(document);
            }

            reader.Close();
            com.Dispose();
            con.Close();
        }

        //public void Change(Document document, bool[] geandert)
        //{
        //    SqlConnection con = new SqlConnection(VERBINDUNG);

        //    con.Open();

        //    string sql = $@"Update document set {(geandert[0] ? $"DatumFaelligkeit = '{r.DatumFaelligkeit}'" : "")}{((geandert[0] && geandert[1] | geandert[2] | geandert[3]) ? "," : "")}" +
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