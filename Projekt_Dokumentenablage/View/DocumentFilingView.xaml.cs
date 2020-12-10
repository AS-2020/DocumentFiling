using Projekt_Dokumentenablage.Models;
using Projekt_Dokumentenablage.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekt_Dokumentenablage.View
{
    /// <summary>
    /// Interaction logic for DocumentFilingView.xaml
    /// </summary>
    public partial class DocumentFilingView : Window
    {
        public DocumentFilingView()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (MainVM)DataContext;
            viewmodel.SelectedDocument = DocumentList.SelectedItems.Cast<Document>().ToList();
        }

        private void FloorSelected(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (MainVM)DataContext;
            var list = new ObservableCollection<string>();
            foreach (var item in StorageLocationHandler.Instance.GetStorageLocations())
            {
                if (item.Floor == viewmodel.Floor)
                {
                    list.Add(item.RoomNumber);
                }
            }
            viewmodel.RoomSelect = list;
            viewmodel.RoomNumber = null;
            viewmodel.ShelfNumber = null;
            viewmodel.Shelf = null;
        }
        private void RoomSelected(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (MainVM)DataContext;
            var list = new ObservableCollection<string>();
            foreach (var item in StorageLocationHandler.Instance.GetStorageLocations())
            {
                if (item.RoomNumber == viewmodel.RoomNumber && item.Floor == viewmodel.Floor)
                {
                    list.Add(item.ShelfNumber);
                }
            }
            viewmodel.ShelfNumberSelect = list;
            viewmodel.ShelfNumber = null;
            viewmodel.Shelf = null;
        }
        private void ShelfNumberSelected(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (MainVM)DataContext;
            var list = new ObservableCollection<string>();
            foreach (var item in StorageLocationHandler.Instance.GetStorageLocations())
            {
                if (item.ShelfNumber == viewmodel.ShelfNumber && item.RoomNumber == viewmodel.RoomNumber && item.Floor == viewmodel.Floor)
                {
                    list.Add(item.Shelf);
                }
            }
            viewmodel.ShelfSelect = list;
            viewmodel.Shelf = null;
        }

        private void NameSelected(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (MainVM)DataContext;
            var list = new ObservableCollection<string>();
            foreach (var item in ResponsiblePersonHandler.Instance.GetResponsiblePerson())
            {
                if (item.Name == viewmodel.Name)
                {
                    list.Add(item.OfficeNumber);
                }
            }
            viewmodel.OfficeNumberSelect = list;
            viewmodel.OfficeNumber = null;
            viewmodel.Department = null;
        }

        private void OfficeNumberSelected(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (MainVM)DataContext;
            var list = new ObservableCollection<string>();
            foreach (var item in ResponsiblePersonHandler.Instance.GetResponsiblePerson())
            {
                if (item.OfficeNumber == viewmodel.OfficeNumber && item.Name == viewmodel.Name)
                {
                    list.Add(item.Department);
                }
            }
            viewmodel.DepartmentSelect = list;
            viewmodel.Department = null;
        }
    }
}
