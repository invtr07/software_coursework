using System;
using System.Collections.Generic;

using Xamarin.Forms;
using static coursework.App;

namespace coursework
{	
	public partial class DecisionsPage : ContentPage
	{
		private int selectedParentIndex;
		public DecisionsPage (string criterion, int index)
		{
			InitializeComponent ();
			
			Title = criterion;

			selectedParentIndex = index + 1;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Listview1.ItemsSource = ahpJson[selectedParentIndex].Children;
			parentDescription.Text = ahpJson[selectedParentIndex].Description;
		}

		void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			string tappedItem = (string)e.Item;
			
			Navigation.PushAsync(new DecisionDescription(tappedItem), true);
		}
    }
}

