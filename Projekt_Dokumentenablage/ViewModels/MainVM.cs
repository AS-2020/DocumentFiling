using Projekt_Dokumentenablage.Helper;
using Projekt_Dokumentenablage.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projekt_Dokumentenablage.ViewModels
{
    class MainVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Document> SelectedDocument { get; set; }

        private ObservableCollection<Document> documentsView;
        public ObservableCollection<Document> DocumentView
        {
            get { return documentsView; }
            set
            {
                documentsView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DocumentView"));
            }
        }
        private ObservableCollection<StorageLocation> locationsView;
        public ObservableCollection<StorageLocation> LocationsView
        {
            get { return locationsView; }
            set
            {
                locationsView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LocationsView"));
            }
        }

        private bool _rechnungsNummerTextBox;
        public bool RechnungsNummerTextBox
        {
            get { return _rechnungsNummerTextBox; }
            set
            {
                _rechnungsNummerTextBox = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RechnungsNummerTextBox"));
            }
        }
        private string _sortieren;
        public string Sortieren
        {
            get { return _sortieren; }
            set
            {
                _sortieren = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sortieren"));
            }
        }

        private bool _aendernButton;
        public bool AendernButton
        {
            get { return _aendernButton; }
            set
            {
                _aendernButton = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AendernButton"));
            }
        }

        private string documentSearch = "";
        public string DocumentSearch
        {
            get { return documentSearch; }
            set
            {
                documentSearch = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DocumentSearch"));
            }
        }

        private int documentNumber;
        public int DocumentNumber
        {
            get { return documentNumber; }
            set
            {
                documentNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DocumentNumber"));
            }
        }

        private string creationDate;
        public string CreationDate
        {
            get { return creationDate; }
            set
            {
                creationDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CreationDate"));
            }
        }

        private StorageLocation location;
        public StorageLocation Location
        {
            get { return location; }
            set
            {
                location = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Location"));
            }
        }
        private ResponsiblePerson person;
        public ResponsiblePerson Person
        {
            get { return person; }
            set
            {
                person = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Person"));
            }
        }
        private string briefDescription;
        public string BriefDescription
        {
            get { return briefDescription; }
            set
            {
                briefDescription = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BriefDescription"));
            }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        private string officeNumber;
        public string OfficeNumber
        {
            get { return officeNumber; }
            set
            {
                officeNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OfficeNumber"));
            }
        }
        private string department;
        public string Department
        {
            get { return department; }
            set
            {
                department = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Department"));
            }
        }
        private string floor;
        public string Floor
        {
            get { return floor; }
            set
            {
                floor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Floor"));
            }
        }
        private string roomNumber;
        public string RoomNumber
        {
            get { return roomNumber; }
            set
            {
                roomNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoomNumber"));
            }
        }
        private string shelf;
        public string Shelf
        {
            get { return shelf; }
            set
            {
                shelf = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Shelf"));
            }
        }
        private string shelfNumber;
        public string ShelfNumber
        {
            get { return shelfNumber; }
            set
            {
                shelfNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ShelfNumber"));
            }
        }



        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ExitCommand { get; set; }
        public RelayCommand ChangeCommand { get; set; }
        public RelayCommand SelectCommand { get; set; }
        public RelayCommand SearchCommand { get; set; }
        public RelayCommand SortCommand { get; set; }
        public void Cancel(object o)
        {
            DocumentNumber = 0;
            CreationDate = null;
            Location = null;
            Person = null;
            BriefDescription = null;
            AendernButton = false;
            RechnungsNummerTextBox = false;
        }

        public void Exit(object o)
        {
            Environment.Exit(0);
        }

        public void SelectDocument(object o)
        {
            try
            {
                Document d = SelectedDocument.Last<Document>();
                DocumentNumber = d.DocumentNumber;
                CreationDate = d.CreationDate.ToShortDateString();
                Location = d.Location;
                Person = d.Person;
                BriefDescription = d.BriefDescription;
                AendernButton = true;
                RechnungsNummerTextBox = true;

            }
            catch (System.ArgumentNullException)
            {

                MessageBox.Show("No document selected");
            }
        }

        public MainVM()
        {
            StorageLocationHandler.Instance.Load();
            ResponsiblePersonHandler.Instance.Load();
            DocumentHandler.Instance.Load();
            documentsView = new ObservableCollection<Document>(DocumentHandler.Instance.GetDocument().OrderBy(r => r.DocumentNumber));

            SaveCommand = new RelayCommand((o) =>
            {
                Document vorhanden = DocumentHandler.Instance.GetDocument().Find(r => r.DocumentNumber == documentNumber);
                if (vorhanden == null && documentNumber > 0 && Location != null && StorageLocationHandler.Instance.GetStorageLocations().Find(l => l == Location) != null
                && Person != null && ResponsiblePersonHandler.Instance.GetResponsiblePerson().Find(p => p == Person) != null)
                {
                    Document d = new Document()
                    {
                        DocumentNumber = DocumentNumber,
                        CreationDate = DateTime.Parse(CreationDate),
                        Location = Location,
                        Person = Person,
                        BriefDescription = BriefDescription
                    };
                    DocumentHandler.Instance.AddDocument(d);
                    documentsView.Add(d);
                    DocumentHandler.Instance.Save(d);
                    Cancel(o);
                }
                else if (vorhanden != null)
                {
                    MessageBox.Show("Document already exists!");
                }
                else if (documentNumber <= 0)
                {
                    MessageBox.Show("Document number can not be under 0!");
                }
                else if (Location == null)
                {
                    MessageBox.Show("Location missing!");
                }
                else if (StorageLocationHandler.Instance.GetStorageLocations().Find(l => l == Location) == null)
                {
                    MessageBox.Show($"There is no {Location}");
                }
                // Person fehlt noch messagebox
            });

            CancelCommand = new RelayCommand(Cancel);

            ExitCommand = new RelayCommand(Exit);

            SearchCommand = new RelayCommand((o) =>
            {
                DocumentView = new ObservableCollection<Document>(DocumentHandler.Instance.GetDocument().FindAll(d => d.DocumentNumber.Equals(DocumentSearch)));
            });

            SortCommand = new RelayCommand((o) =>
            {
                if (Sortieren == "Rechnung sortieren")
                {
                    DocumentView = new ObservableCollection<Document>(DocumentHandler.Instance.GetDocument().OrderBy(r => r.DocumentNumber));
                    Sortieren = "Kunden sortieren";
                }
                else if (Sortieren == "Kunden sortieren")
                {
                    DocumentView = new ObservableCollection<Document>(DocumentHandler.Instance.GetDocument().OrderBy(r => r.DocumentNumber));
                    Sortieren = "Rechnung sortieren";
                }
            });

            SelectCommand = new RelayCommand(SelectDocument);

            ChangeCommand = new RelayCommand((o) =>
            {
                Document vorhanden = DocumentHandler.Instance.GetDocument().Find(r => r.DocumentNumber == DocumentNumber);
                bool[] geandert = new bool[4] { false, false, false, false };
                if (vorhanden == null && documentNumber > 0 && Location != null && StorageLocationHandler.Instance.GetStorageLocations().Find(l => l == Location) != null
                && Person != null && ResponsiblePersonHandler.Instance.GetResponsiblePerson().Find(p => p == Person) != null)
                {
                    Document d = new Document()
                    {
                        DocumentNumber = DocumentNumber,
                        CreationDate = DateTime.Parse(CreationDate),
                        Location = Location,
                        Person = Person,
                        BriefDescription = BriefDescription
                    };
                    if (d.CreationDate != vorhanden.CreationDate)
                    {
                        geandert[0] = true;
                    }
                    if (d.Location != vorhanden.Location)
                    {
                        geandert[1] = true;
                    }
                    if (d.Person != vorhanden.Person)
                    {
                        geandert[2] = true;
                    }
                    if (d.BriefDescription != vorhanden.BriefDescription)
                    {
                        geandert[3] = true;
                    }
                    DocumentHandler.Instance.RemoveDocument(vorhanden);
                    DocumentHandler.Instance.AddDocument(d);
                    documentsView.Add(d);
                    //DocumentHandler.Instance.Change(d, geandert);
                    Cancel(o);
                    DocumentView = new ObservableCollection<Document>(DocumentHandler.Instance.GetDocument().OrderBy(docu => docu.DocumentNumber));
                }
                else if (vorhanden != null)
                {
                    MessageBox.Show("Document already exists!");
                }
                else if (documentNumber <= 0)
                {
                    MessageBox.Show("Document number can not be under 0!");
                }
                else if (Location == null)
                {
                    MessageBox.Show("Location missing!");
                }
                else if (StorageLocationHandler.Instance.GetStorageLocations().Find(l => l == Location) == null)
                {
                    MessageBox.Show($"There is no {Location}");
                }
            });
        }
    }
}