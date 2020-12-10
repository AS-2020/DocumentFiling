using Projekt_Dokumentenablage.Models;
using Projekt_Dokumentenablage.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for NewPersonView.xaml
    /// </summary>
    public partial class NewPersonView : Window
    {
        public NewPersonView()
        {
            InitializeComponent();
        }
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = (NewPersonVM)DataContext;
            viewmodel.SelectedPerson = PersonsList.SelectedItems.Cast<ResponsiblePerson>().ToList();
        }
    }
}
