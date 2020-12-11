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
    class NewLocationVM : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public List<StorageLocation> SelectedLocation { get; set; }

        private ObservableCollection<StorageLocation> _locations;
        public ObservableCollection<StorageLocation> Locations
        {
            get { return _locations; }
            set
            {
                _locations = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Locations"));
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
        private string _sort;
        public string Sort
        {
            get { return _sort; }
            set
            {
                _sort = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sort"));
            }
        }

        private bool _changeButton;
        public bool ChangeButton
        {
            get { return _changeButton; }
            set
            {
                _changeButton = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ChangeButton"));
            }
        }

        private string _searchLocation = "";
        public string SearchLocation
        {
            get { return _searchLocation; }
            set
            {
                _searchLocation = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchLocation"));
            }
        }
        private int _locationID;
        public int LocationID
        {
            get { return _locationID; }
            set
            {
                _locationID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("LocationID"));
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
            Floor = null;
            RoomNumber = null;
            ShelfNumber = null;
            Shelf = null;
            ChangeButton = false;
            RechnungsNummerTextBox = false;
        }

        public void Exit(object o)
        {
            Environment.Exit(0);
        }

        public void SelectLocation(object o)
        {
            try
            {
                StorageLocation p = SelectedLocation.Last<StorageLocation>();
                LocationID = p.LocationID;
                Floor = p.Floor;
                RoomNumber = p.RoomNumber;
                ShelfNumber = p.ShelfNumber;
                Shelf = p.Shelf;
                ChangeButton = true;
                RechnungsNummerTextBox = true;

            }
            catch (System.ArgumentNullException)
            {

                MessageBox.Show("No storage location selected");
            }

        }

        public NewLocationVM()
        {
            Locations = new ObservableCollection<StorageLocation>(StorageLocationHandler.Instance.GetStorageLocations().OrderBy(p => p.Floor));

            SaveCommand = new RelayCommand((o) =>
            {
                StorageLocation vorhanden = StorageLocationHandler.Instance.GetStorageLocations().Find(r => r.Floor == Floor && r.RoomNumber == RoomNumber && r.ShelfNumber == shelfNumber && r.Shelf == Shelf);
                if (vorhanden == null && Floor != null && RoomNumber != null && ShelfNumber != null && Shelf == null)
                {
                    StorageLocation r = new StorageLocation()
                    {
                        Floor = Floor,
                        RoomNumber = RoomNumber,
                        ShelfNumber = ShelfNumber,
                        Shelf = Shelf
                    };
                    StorageLocationHandler.Instance.AddStorageLocation(r);
                    Locations.Add(r);
                    StorageLocationHandler.Instance.Save(r);
                    Cancel(o);

                }
                else if (vorhanden != null)
                {
                    MessageBox.Show("This storage location already exists");
                }
                else if (Floor == null)
                {
                    MessageBox.Show("Enter a floor");
                }
                else if (RoomNumber == null)
                {
                    MessageBox.Show("Enter the room number");
                }
                else if (ShelfNumber == null)
                {
                    MessageBox.Show($"Enter the shelf number");
                }
                else if (Shelf == null)
                {
                    MessageBox.Show($"Enter the shelf compartment");
                }
            });

            CancelCommand = new RelayCommand(Cancel);

            ExitCommand = new RelayCommand(Exit);

            SearchCommand = new RelayCommand((o) =>
            {
                Locations = new ObservableCollection<StorageLocation>(StorageLocationHandler.Instance.GetStorageLocations().FindAll(r => r.Floor.ToLower().Contains(Floor.ToLower())));
            });

            SortCommand = new RelayCommand((o) =>
            {
                if (Sort == "Sort")
                {
                    Locations = new ObservableCollection<StorageLocation>(StorageLocationHandler.Instance.GetStorageLocations().OrderBy(r => r.Floor));
                    Sort = "Sort";
                }
                else if (Sort == "Kunden sortieren")
                {
                //Rechnungen = new ObservableCollection<Rechnung>(Buchhaltung.Instance.GetRechnungen().OrderBy(r => r.KundenNummer));
                //Sortieren = "Rechnung sortieren";
            }
            });

            SelectCommand = new RelayCommand(SelectLocation);

            ChangeCommand = new RelayCommand((o) =>
            {
                bool[] geandert = new bool[4] { false, false, false, false };
                StorageLocation vorhanden = StorageLocationHandler.Instance.GetStorageLocations().Find(r => r.LocationID == LocationID);
                if (Floor != null && RoomNumber != null && ShelfNumber != null && Shelf != null)
                {
                    StorageLocation r = new StorageLocation()
                    {
                        LocationID = vorhanden.LocationID,
                        Floor = floor,
                        RoomNumber = RoomNumber,
                        ShelfNumber = ShelfNumber,
                        Shelf = Shelf
                    };
                    if (vorhanden != null)
                    {
                        if (vorhanden.Floor == null || r.Floor != vorhanden.Floor)
                        {
                            geandert[0] = true;
                        }
                        if (vorhanden.RoomNumber == null || r.RoomNumber != vorhanden.RoomNumber)
                        {
                            geandert[1] = true;
                        }
                        if (vorhanden.ShelfNumber == null || r.ShelfNumber != vorhanden.ShelfNumber)
                        {
                            geandert[2] = true;
                        }
                        if (vorhanden.Shelf == null || r.Shelf != vorhanden.Shelf)
                        {
                            geandert[3] = true;
                        }
                    }
                    else
                    {
                        geandert[0] = true;
                        geandert[1] = true;
                        geandert[2] = true;
                        geandert[3] = true;
                    }

                    StorageLocationHandler.Instance.RemoveStorageLocation(vorhanden);
                    StorageLocationHandler.Instance.AddStorageLocation(r);
                    StorageLocationHandler.Instance.Change(r, geandert);
                    Locations = new ObservableCollection<StorageLocation>(StorageLocationHandler.Instance.GetStorageLocations().OrderBy(re => re.Floor));
                    Cancel(o);
                }
                else if (vorhanden == null)
                {
                    MessageBox.Show("This person does not exists");
                }
                else if (Floor == null)
                {
                    MessageBox.Show("Enter the floor");
                }
                else if (RoomNumber == null)
                {
                    MessageBox.Show("Enter the room number");
                }
                else if (ShelfNumber == null)
                {
                    MessageBox.Show($"Enter the shelf number");
                }
                else if (Shelf == null)
                {
                    MessageBox.Show($"Enter the shelf compartment");
                }
            });
        }
    }
}