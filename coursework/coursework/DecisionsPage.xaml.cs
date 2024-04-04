using System;
using System.Collections.Generic;

using Xamarin.Forms;
using static coursework.App;

namespace coursework
{	
	public partial class DecisionsPage : ContentPage
	{
		
		public DecisionsPage (string parent)
		{
			InitializeComponent ();

			Title = parent;

            // Find the ParentNode corresponding to the specified parent using foreach loop
            ParentNode parentNode = null;
            foreach (var criterion in App.criteria)
            {
                if (criterion.Name == parent)
                {
                    parentNode = criterion;
                    break;
                }
            }

            // Set the BindingContext of the page to the found ParentNode
            BindingContext = parentNode;
        }

        void ListView_ItemTapped(System.Object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                string tappedItem = e.Item.ToString();

                DisplayAlert("Item Tapped", $"You tapped on: {tappedItem}", "OK");
            }

        }
    }
}

