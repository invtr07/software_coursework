using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace coursework
{	
	public partial class DecisionDescription : ContentPage
	{	
		public DecisionDescription (string decision)
		{
			InitializeComponent ();

			Title = decision;
		}
	}
}

