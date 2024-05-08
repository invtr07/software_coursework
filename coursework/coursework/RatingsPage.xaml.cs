using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace coursework
{	
	public partial class RatingsPage : ContentPage
	{	
		
		public RatingsPage ()
		{
			InitializeComponent ();
            Title = App.HierarchyData.FirstOrDefault()?.Name;
            ValidateAndDisplayRatings();
        }

		private void ValidateAndDisplayRatings()
        {
            var missingEvaluations = new List<string>();
            foreach (var node in App.HierarchyData)
            {
                if (node.Children.Count > 0 && (node.LocalPriorities == null || node.LocalPriorities.Count != node.Children.Count))
                {
                    missingEvaluations.Add(node.Name);
                }
            }
            
            if (missingEvaluations.Any())
            {
                string firstMissingNode = missingEvaluations.First();
                int additionalMissingCount = missingEvaluations.Count - 1; // Exclude the first one from the additional count

                // If there are more than one missing evaluations, include the count of additional nodes
                string additionalNodesText = additionalMissingCount > 0 ? $" and {additionalMissingCount} more nodes" : "";

                EvaluationValidationLabel.Text = $"'{firstMissingNode}'{additionalNodesText} require evaluation.";
            }
            else
            {
                DisplayRatings();
            }
        }
        
        private void DisplayRatings()
        {
            var alternatives = App.HierarchyData.Where(node => node.Children.Count == 0).ToList(); // Assuming leaf nodes are alternatives
            var ratings = alternatives.Select(alt => new DecisionGlobalRating
            {
                Name = alt.Name,
                GlobalImportance = CalculateGlobalImportance(alt)
            }).ToList();
        
            DecisionRatingList.ItemsSource = ratings;
        }
        
        private double CalculateGlobalImportance(App.Node node)
        {
            double globalImportance = 0;
            var current = node;
            while (current != null)
            {
                var parent = App.HierarchyData.FirstOrDefault(n => n.Children.Contains(current));
                if (parent != null)
                {
                    int index = parent.Children.IndexOf(current);
                    double localImportance = parent.LocalPriorities[index];
                    globalImportance += localImportance;
                    current = parent;
                }
                else
                {
                    current = null;
                }
            }
            return globalImportance;
        }
		public class DecisionGlobalRating
		{
			public string Name { get; set; }
			public double GlobalImportance { get; set; }
		}
	}
}

