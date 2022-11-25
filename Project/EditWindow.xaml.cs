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
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : Window
    {
        public Inventory MyInventory { get; private set; }
        public EditWindow()
        {
            InitializeComponent();
        }
        public EditWindow(Inventory i)
        {
            Item item = new Item();
            MyInventory = i;
            InitializeComponent();
            Name.ItemsSource = i.GetName();
            Supplier.ItemsSource = item.SupplierList;
            sCategory.ItemsSource = Enum.GetValues(typeof(Category)).Cast<Category>();
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {

            if (CheckEditInput())
            {
                MyInventory.Items.ElementAt(Name.SelectedIndex).Quantity = int.Parse(txtQuantity.Text);
                MyInventory.Items.ElementAt(Name.SelectedIndex).Minimum = int.Parse(txtMinQuantity.Text);
                MyInventory.Items.ElementAt(Name.SelectedIndex).Location = txtLocation.Text;
                MyInventory.Items.ElementAt(Name.SelectedIndex).Supplier = Supplier.SelectedItem.ToString();

                Category c = (Category)Enum.Parse(typeof(Category), sCategory.SelectedItem.ToString());
                MyInventory.Items.ElementAt(Name.SelectedIndex).ItemCategory = c;

                this.Close();
            }
        }

        private void Name_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtQuantity.Text = MyInventory.Items.ElementAt(Name.SelectedIndex).Quantity.ToString();
            txtMinQuantity.Text = MyInventory.Items.ElementAt(Name.SelectedIndex).Minimum.ToString();
            txtLocation.Text = MyInventory.Items.ElementAt(Name.SelectedIndex).Location;
            Supplier.SelectedItem = MyInventory.Items.ElementAt(Name.SelectedIndex).Supplier;
            sCategory.SelectedItem = MyInventory.Items.ElementAt(Name.SelectedIndex).ItemCategory;
        }
        private bool CheckEditInput()
        {
            StringBuilder warning = new StringBuilder();

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
