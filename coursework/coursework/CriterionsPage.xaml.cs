using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace coursework
{	
	public partial class CriterionsPage : ContentPage
	{
		// public ObservableCollection<ViewModels.Goal> goal;
		public CriterionsPage ()
		{
			InitializeComponent ();
			// goal = new ObservableCollection<ViewModels.Goal>();
			Title = App.ahpJson[0].Name;
			goalDescription.Text=App.ahpJson[0].Description;
		}
	
		protected override void OnAppearing()
		{
			base.OnAppearing();
			
            ListView.ItemsSource = App.ahpJson[0].Children;
		}

		void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            // Get the tapped item
            
            string tappedItem = (string)e.Item;
            int tappedItemIndex = e.ItemIndex;
            Navigation.PushAsync(new DecisionsPage(tappedItem, tappedItemIndex), true);
        }
    }
}

