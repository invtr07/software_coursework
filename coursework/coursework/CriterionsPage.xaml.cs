using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace coursework
{	
	public partial class CriterionsPage : ContentPage
	{

		public CriterionsPage ()
		{
			InitializeComponent ();

			var goalArr = App.goal[0];
			string[] goalChildren = goalArr.Children;

            Title = goalArr.Name;
			goalDescription.Text = goalArr.Description;

			ListView.ItemsSource = goalChildren;
		}

        void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            // Get the tapped item
            string tappedItem = (string)e.Item;

            Navigation.PushAsync(new DecisionsPage(tappedItem), true);
        }
    }
}

