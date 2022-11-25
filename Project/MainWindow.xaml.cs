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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Project.Models;
using System.IO;
using Microsoft.Win32;
using static Project.Models.Item;

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window //not many comments in this project as method names are self explanatory
    {
        private string saveLocation = string.Empty; 
        private bool saved = false;

        Inventory i = new Inventory();
        
        public MainWindow()
        {
            InitializeComponent();
            RetrieveItems();

            lbItems.ItemsSource = i.Items; //binding
            lbQuantity.ItemsSource = i.Items;
            lbCategory.ItemsSource = i.Items;
            lbLocation.ItemsSource = i.Items;
            lbSupplier.ItemsSource = i.Items;
        }
        private void RetrieveItems()
        {
            i.LoadItems("./test.txt"); // if you want something loaded by default.
        }

        //private void lbItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void lbQuantity_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void lbCategory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void lbLocation_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{

        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //}

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            
            if (string.IsNullOrEmpty(saveLocation))
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "TXT Files|*.txt";
                if (save.ShowDialog() == true)
                {
                    saveLocation = save.FileName;
                }
                else
                    return;
            }

            SaveDataFile();
        }

        private void SaveDataFile()
        {
            if (!saved)
            {
                try
                {
                    StringBuilder allItemsInfo = new StringBuilder();
                    foreach (Item item in i.Items)
                    {
                        allItemsInfo.AppendLine(item.TXTData);
                    }

                    File.WriteAllText(saveLocation, allItemsInfo.ToString());

                    MessageBox.Show("Saved");
                    saved = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                MessageBox.Show("No unsaved data.");
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBeforOpen())
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "TXT Files|*.txt";

                if (open.ShowDialog() == true)
                {
                    saveLocation = open.FileName;
                    saved = true;


                    i.Items.Clear();
                    ReadItems();

                    RefreshAll();
                }
            }
        }

        private bool CheckBeforOpen()
        {
            if (string.IsNullOrEmpty(saveLocation))
            {
                return true;
            }

            if (saved)
            {
                return true;
            }

            MessageBoxResult result = MessageBox.Show("Do you want to save changes?", "Unsaved data", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.No)
            {
                return true;
            }

            if (result == MessageBoxResult.Cancel)
            {
                return false;
            }

            if (result == MessageBoxResult.Yes)
            {
                SaveLogic();
            }

            return saved;
        }

        private void SaveLogic()
        {
            if (string.IsNullOrEmpty(saveLocation))
            {
                SaveFileDialog save = new SaveFileDialog();
                save.Filter = "TXT Files|*.txt";
                if (save.ShowDialog() == true)
                {
                    saveLocation = save.FileName;
                }
                else
                {
                    return;
                }
            }
            SaveDataFile();
        }

        private void ReadItems()
        {
            if (File.Exists(saveLocation))
            {
                try
                {
                    string[] values = File.ReadAllLines(saveLocation);
                    foreach (string singleItem in values)
                    {
                        string[] itemInfo = singleItem.Split(',');
                        Category c = (Category)Enum.Parse(typeof(Category), itemInfo[4]);
                        Item item = new Item(itemInfo[0], int.Parse(itemInfo[1]), int.Parse(itemInfo[2]), itemInfo[3], c, itemInfo[5]); 
                        i.AddItem(item);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Data was in incorrect format or contains unexpected values\n\nExpected format: \nName,Quantity,Minimum Quantity,Category,Supplier\n\nExpected values: \nQuantity: positive integer\nMinimum Quantity: positive integer\nCategory: from predefined options", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //throw;
                }

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !CheckBeforOpen();
        }

        private void btnShoppingList_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(i.ShoppingList(), "Shopping List");
        }

        private void btnGeneralReport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(i.GeneralReport(), "General Report");
        }

        private void btnAddToList_Click(object sender, RoutedEventArgs e)
        {
            Item emptyItem = new Item();
            AddWindow add = new AddWindow(emptyItem);
            add.ShowDialog();

            if (add.MyItem.Name != null)
                i.AddItem(add.MyItem); 

            RefreshAll();

            saved = false;
        }

        private void btnRemoveList_Click(object sender, RoutedEventArgs e)
        {
            RemoveWindow remove = new RemoveWindow(i);
            remove.ShowDialog();

            i.RemoveItem(remove.ItemToBeRemoved);

            RefreshAll();

            saved = false;
        }

        private void EditList_Click(object sender, RoutedEventArgs e)
        {
            EditWindow edit = new EditWindow(i);
            edit.ShowDialog();

            RefreshAll();

            saved = false;
        }
        private void RefreshAll()
        {
            lbItems.Items.Refresh(); 
            lbCategory.Items.Refresh();
            lbLocation.Items.Refresh();
            lbQuantity.Items.Refresh();
            lbSupplier.Items.Refresh();
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            if (lbQuantity.SelectedItem != null)
            {
                Item increase = lbQuantity.SelectedItem as Item;

                if (increase != null)
                {
                    increase.Quantity++;
                    RefreshAll();
                    saved = false;
                }
            }
            else
                MessageBox.Show("Please click on a Quantity first.");

        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (lbQuantity.SelectedItem != null)
            {
                Item decrease = lbQuantity.SelectedItem as Item;

                if (decrease != null)
                {
                    if (decrease.Quantity > 0)
                    {
                        decrease.Quantity--;
                        RefreshAll();
                        saved = false;
                    }
                    else
                    {
                        MessageBox.Show("Quantity must be greater than 0");
                    }

                }
            }
            else
                MessageBox.Show("Please click on a Quantity first.");

        }
    }
}