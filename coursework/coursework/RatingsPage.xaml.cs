using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace coursework
{	
	public partial class RatingsPage : ContentPage
	{	
		public RatingsPage ()
		{
			InitializeComponent ();
			Title = App.HierarchyData[0].Name;
		}
	}
}

