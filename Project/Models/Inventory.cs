using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project.Models.Item;

namespace Project.Models
{
    public class Inventory 
    {
        private List<Item> items = new List<Item>();

        public void AddItem(Item newItem)
        {
            items.Add(newItem);
        }
        public void RemoveItem(string itemName)
        {
            items.RemoveAll(item => item.Name == itemName);
        }
        public List<Item> Items 
            { get { return items; } }
        public string GeneralReport() 
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("+==========================+\n");
            foreach (Item item in items)
            {
                s.AppendLine(string.Format("Name: {0} \nAvailable Quantity: {1} \nMinimum Quantity: {2}\n", item.Name, item.Quantity, item.Minimum));
            }
            s.AppendLine("+==========================+");
            return s.ToString();
        }
        public string ShoppingList()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("+==========================+\n");
            foreach (Item item in items)
            {
                if (item.Quantity < item.Minimum)
                    s.AppendLine(string.Format("Name: {0} \nAvailable Quantity: {1} \nMinimum Quantity: {2}\n", item.Name, item.Quantity, item.Minimum));
            }
            s.AppendLine("+==========================+");
            return s.ToString();
        }
        public void LoadItems(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string[] lines = File.ReadAllLines(filePath);

                    foreach(string line in lines)
                    {
                        string[] itemInfo = line.Split(',');

                        Category c = (Category)Enum.Parse(typeof(Category), itemInfo[4]);

                        Item i = new Item(itemInfo[0], int.Parse(itemInfo[1]), int.Parse(itemInfo[2]), itemInfo[3], c, itemInfo[5]); 

                        items.Add(i);
                    }
                }
                catch (Exception)
                {
                    
                }
            }
        }
        public List<string> GetName()
        {
            List<string> names = new List<string>();

            foreach (Item item in items)
            {
                names.Add(item.Name);
            }

            return names;
        }
        
    }
}