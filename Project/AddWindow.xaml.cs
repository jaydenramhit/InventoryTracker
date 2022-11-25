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
using static Project.Models.Item;

namespace Project
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public Item MyItem { get; private set; }
        public AddWindow()
        {
            InitializeComponent();
        }
        public AddWindow(Item item)
        {
            Item i = new Item();
            InitializeComponent();
            MyItem = item;
            Supplier.ItemsSource = i.SupplierList; //binding dropdown lists
            sCategory.ItemsSource = Enum.GetValues(typeof(Category)).Cast<Category>();
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAddInput())
            {
                MyItem.Name = txtName.Text;
                MyItem.Quantity = int.Parse(txtQuantity.Text);
                MyItem.Minimum = int.Parse(txtMinQuantity.Text);
                MyItem.Location = txtLocation.Text;
                MyItem.Supplier = Supplier.SelectedItem.ToString();

                Category c = (Category)Enum.Parse(typeof(Category), sCategory.SelectedItem.ToString());
                MyItem.ItemCategory = c;

                this.Close();
            }
        }
        private bool CheckAddInput()
        {
            StringBuilder warning = new StringBuilder();

            if (string.IsNullOrEmpty(txtName.Text))
                warning.AppendLine("Name is a required field.");

            bool successQty = uint.TryParse(txtQuantity.Text, out uint quantity);
            if (string.IsNullOrEmpty(txtQuantity.Text) || !successQty)
                warning.AppendLine("Quantity is a required field, enter a postive integer.");

            bool successMinQty = uint.TryParse(txtMinQuantity.Text, out uint minQuantity);
            if (string.IsNullOrEmpty(txtMinQuantity.Text) || !successMinQty)
                warning.AppendLine("Minimum quantity is a required field, enter a postive integer.");
            
            if (Supplier.SelectedIndex == -1)
                warning.AppendLine("Supplier is a required field.");
            
            if (sCategory.SelectedIndex == -1)
                warning.AppendLine("Category is a required field.");

            if (string.IsNullOrEmpty(warning.ToString()))
                return true;

            MessageBox.Show(warning.ToString(), "Missing Fields", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }
}
