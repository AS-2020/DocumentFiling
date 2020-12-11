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
    class NewPersonVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<ResponsiblePerson> SelectedPerson { get; set; }

        private ObservableCollection<ResponsiblePerson> _persons;
        public ObservableCollection<ResponsiblePerson> Persons
        {
            get { return _persons; }
            set
            {
                _persons = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Persons"));
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

        private string _searchPerson = "";
        public string SearchPerson
        {
            get { return _searchPerson; }
            set
            {
                _searchPerson = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SearchPerson"));
            }
        }
        private int _personID;
        public int PersonID
        {
            get { return _personID; }
            set
            {
                _personID = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PersonID"));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }

        private string _officeNumber;
        public string OfficeNumber
        {
            get { return _officeNumber; }
            set
            {
                _officeNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OfficeNumber"));
            }
        }


        private string _department;
        public string Department
        {
            get { return _department; }
            set
            {
                _department = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Department"));
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
            Name = null;
            OfficeNumber = null;
            Department = null;
            ChangeButton = false;
            RechnungsNummerTextBox = false;
        }

        public void Exit(object o)
        {
            Environment.Exit(0);
        }

        public void SelectPerson(object o)
        {
            try
            {
                ResponsiblePerson p = SelectedPerson.Last<ResponsiblePerson>();
                PersonID = p.PersonID;
                Name = p.Name;
                OfficeNumber = p.OfficeNumber;
                Department = p.Department;
                ChangeButton = true;
                RechnungsNummerTextBox = true;

            }
            catch (System.ArgumentNullException)
            {

                MessageBox.Show("No responsible Person selected");
            }

        }

        public NewPersonVM()
        {
            Persons = new ObservableCollection<ResponsiblePerson>(ResponsiblePersonHandler.Instance.GetResponsiblePerson().OrderBy(p => p.Name));

            SaveCommand = new RelayCommand((o) =>
            {
                ResponsiblePerson vorhanden = ResponsiblePersonHandler.Instance.GetResponsiblePerson().Find(r => r.Name == Name && r.OfficeNumber == OfficeNumber && r.Department == Department);
                if (vorhanden == null && Name != null && OfficeNumber != null && Department != null)
                {
                    ResponsiblePerson r = new ResponsiblePerson()
                    {
                        Name = Name,
                        OfficeNumber = OfficeNumber,
                        Department = Department
                    };
                    ResponsiblePersonHandler.Instance.AddResponsiblePerson(r);
                    Persons.Add(r);
                    ResponsiblePersonHandler.Instance.Save(r);
                    Cancel(o);

                }
                else if (vorhanden != null)
                {
                    MessageBox.Show("This person already exists");
                }
                else if (Name == null)
                {
                    MessageBox.Show("Enter a name");
                }
                else if (OfficeNumber == null)
                {
                    MessageBox.Show("Enter the office Number");
                }
                else if (Department == null)
                {
                    MessageBox.Show($"Enter the department");
                }
            });

            CancelCommand = new RelayCommand(Cancel);

            ExitCommand = new RelayCommand(Exit);

            SearchCommand = new RelayCommand((o) =>
                {
                    Persons = new ObservableCollection<ResponsiblePerson>(ResponsiblePersonHandler.Instance.GetResponsiblePerson().FindAll(r => r.Name.ToLower().Contains(SearchPerson.ToLower())));
                });

            SortCommand = new RelayCommand((o) =>
            {
                if (Sort == "Sort")
                {
                    Persons = new ObservableCollection<ResponsiblePerson>(ResponsiblePersonHandler.Instance.GetResponsiblePerson().OrderBy(r => r.Name));
                    Sort = "Sort";
                }
                else if (Sort == "Kunden sortieren")
                {
                    //Rechnungen = new ObservableCollection<Rechnung>(Buchhaltung.Instance.GetRechnungen().OrderBy(r => r.KundenNummer));
                    //Sortieren = "Rechnung sortieren";
                }
            });

            SelectCommand = new RelayCommand(SelectPerson);

            ChangeCommand = new RelayCommand((o) =>
                {
                    bool[] geandert = new bool[3] { false, false, false };
                    ResponsiblePerson vorhanden = ResponsiblePersonHandler.Instance.GetResponsiblePerson().Find(r => r.PersonID == PersonID);
                    if (Name != null && OfficeNumber != null && Department != null)
                    {
                        ResponsiblePerson r = new ResponsiblePerson()
                        {
                            PersonID = vorhanden.PersonID,
                            Name = Name,
                            OfficeNumber = OfficeNumber,
                            Department = Department
                        };
                        if (vorhanden != null)
                        {
                            if (vorhanden.Name == null || r.Name != vorhanden.Name)
                            {
                                geandert[0] = true;
                            }
                            if (vorhanden.OfficeNumber == null || r.OfficeNumber != vorhanden.OfficeNumber)
                            {
                                geandert[1] = true;
                            }
                            if (vorhanden.Department == null || r.Department != vorhanden.Department)
                            {
                                geandert[2] = true;
                            }
                        }
                        else
                        {
                            geandert[0] = true;
                            geandert[1] = true;
                            geandert[2] = true;
                        }

                        ResponsiblePersonHandler.Instance.RemoveResponsiblePerson(vorhanden);
                        ResponsiblePersonHandler.Instance.AddResponsiblePerson(r);
                        ResponsiblePersonHandler.Instance.Change(r, geandert);
                        Persons = new ObservableCollection<ResponsiblePerson>(ResponsiblePersonHandler.Instance.GetResponsiblePerson().OrderBy(re => re.Name));
                        Cancel(o);
                    }
                    else if (vorhanden == null)
                    {
                        MessageBox.Show("This person does not exists");
                    }
                    else if (Name == null)
                    {
                        MessageBox.Show("Enter a name");
                    }
                    else if (OfficeNumber == null)
                    {
                        MessageBox.Show("Enter the office Number");
                    }
                    else if (Department == null)
                    {
                        MessageBox.Show($"Enter the department");
                    }
                });
        }
    }
}