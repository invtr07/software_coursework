using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace coursework
{	
	public partial class DecisionDescription : ContentPage
	{
		private string decisionName;
		public DecisionDescription (string decision)
		{
			InitializeComponent ();
			
			Title = decision;

			decisionName = decision;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			
			int index = Array.FindIndex(App.ahpJson, obj => obj.Name == decisionName);
			
			Label1.Text = App.ahpJson[index].Description;
		}
	}
}

