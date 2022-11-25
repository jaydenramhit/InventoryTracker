using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Item
    {
        private string name;
        private int quantity;
        private int minimum;
        private string location;
        private List<string> supplierList = new List<string>{"Walmart", "Costco", "Local Farm"}; 
        private string supplier;
        private Category category;

        public Item()
        {

        }
        public Item(string name, int quantity, int minimum, string location, Category category, string supplier)
        {
            Name = name;
            Quantity = quantity;
            Minimum = minimum;
            Location = location;
            ItemCategory = category;
            Supplier = supplier;
        }

        public int Quantity { 
            get { return quantity; } 
            set { if (value >= 0)
                    quantity = value;
                else
                    throw new ArgumentException("quantity must be positive", "quantity");
            } 
        }
        public int Minimum
        {
            get { return minimum; }
            set
            {
                if (value > 0)
                    minimum = value;
                else
                    throw new ArgumentException("minimum cannot be less than 1", "minimum");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }
        public string Location
        {
            get { return location; }
            set
            {
                location = value;
            }
        }
        public string Supplier { 
            get { return supplier; } 
            set { supplier = value;}
        }
        public List<string> SupplierList
        {
            get { return supplierList; }
        }
        public Category ItemCategory 
        {
            get { return category; }
            set { category = value; } 
        }
        public enum Category
        {
            Pantry, Diary, Drinks, Frozen, Fruits, Vegetables, Bakery, Cleaning, Other
        }

        public string TXTData
        {
            get { return string.Format($"{Name},{Quantity},{Minimum},{Location},{ItemCategory},{Supplier}"); }
            set 
            {
                string[] data = value.Split(',');
                Category category = (Category)Enum.Parse(typeof(Category), data[4]);
                try
                {
                    Name = data[0];
                    Quantity = int.Parse(data[1]);
                    Minimum = int.Parse(data[2]);
                    Location = data[3];
                    ItemCategory = category;
                    Supplier = data[5];
                }
                catch (Exception ex)
                {
                    throw new Exception("All Data Property value not valid " + ex.Message);
                }
            }
        }
    }
}
