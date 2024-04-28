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
        public DataModels.MenuItems[] menuItems;
        public MainPage()
        {
            InitializeComponent();

            menuItems = new []
            {
                new DataModels.MenuItems{ Title = "Hierarchy", Descr = "Browse and evaluate" },
                new DataModels.MenuItems{ Title = "Decision alternatives", Descr = "View rating scores" }
            };

            ListView.ItemsSource = menuItems;
        }
        
        void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            DataModels.MenuItems item = (DataModels.MenuItems)e.Item;

            if(item.Title == "Hierarchy")
            {
                Navigation.PushAsync(new CriterionsPage(), true);
            }
            else if(item.Title == "Decision alternatives")
            {
                Navigation.PushAsync(new RatingsPage(), true);
            }

        }
        
    }
}

