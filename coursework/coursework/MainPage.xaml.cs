using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace coursework
{
    public partial class MainPage : ContentPage
    {
        public MenuItems[] menuItems;

        public MainPage()
        {
            InitializeComponent();

            menuItems = new MenuItems[]
            {
                new MenuItems{ Title="Hierarchy", Descr="Browse and evaluate" },
                new MenuItems{ Title="Decision alternatives", Descr="View rating scores" }
            };

            ListView.ItemsSource = menuItems;
        }

        public class MenuItems
        {
            public string Title { get; set; }
            public string Descr { get; set; }

        }
    }
}

