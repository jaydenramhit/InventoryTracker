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
using Project.Models;

namespace Project
{
    /// <summary>
    /// Interaction logic for RemoveWindow.xaml
    /// </summary>
    public partial class RemoveWindow : Window
    {
        public string ItemToBeRemoved { get; private set; }
        public RemoveWindow()
        {
            InitializeComponent();
        }
        public RemoveWindow(Inventory i)
        {
            InitializeComponent();
            Name.ItemsSource = i.GetName();
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            ItemToBeRemoved = Name.SelectedItem.ToString();
            this.Close();
        }
    }
}
