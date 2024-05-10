﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Xamarin.Essentials;
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
                //validate if all nodes have evaluations
                if (node.Children.Count > 0 && (node.LocalPriorities == null || node.LocalPriorities.Count != node.Children.Count))
                {
                    missingEvaluations.Add(node.Name);
                }
            }
            
            if (missingEvaluations.Any())
            {
                string firstMissingNode = missingEvaluations.First();
                int additionalMissingCount = missingEvaluations.Count - 1;
                // Excludes the first one from the additional count

                // If there are more than one missing evaluations, include the count of additional nodes
                string additionalNodesText = additionalMissingCount > 0 ? $" and {additionalMissingCount} more nodes" : "";

                HeaderLabel.Text = "Ratings of the alternatives are not available:";
                EvaluationValidationLabel.Text = $"'{firstMissingNode}'{additionalNodesText} require evaluation.";
                
                DisplayDecisionsWithoutRatings();
                SubmitBtn.IsVisible = false;
            }
            else
            {
                DisplayRatings();
                HeaderLabel.Text = "Ratings of the alternatives:";
                SubmitBtn.IsVisible = true;
            }
        }
        
        private void DisplayDecisionsWithoutRatings() 
        {
            var alternatives = App.HierarchyData.Where(node => node.Children.Count == 0).ToList(); // Assuming leaf nodes are alternatives
            var ratings = alternatives.Select(alt => new DecisionGlobalRating
            {
                DecisionName = alt.Name,
                GlobalImportance = 0
            }).ToList();
        
            DecisionRatingList.ItemsSource = ratings;
        }
        
        private void DisplayRatings()
        {
            var alternatives = App.HierarchyData.Where(node => node.Children.Count == 0).ToList(); // Assuming leaf nodes are alternatives
            var ratings = alternatives.Select(alt => new DecisionGlobalRating
            {
                DecisionName = alt.Name,
                GlobalImportance = CalculateGlobalImportance(alt) //passing the leafnode
            }).ToList();
        
            DecisionRatingList.ItemsSource = ratings; //binding
        }
        
        private double CalculateGlobalImportance(App.Node node)
        {
            // This will recursively calculate the global importance
            return CalculateGlobalImportanceRecursive(node, 1); // Start with initial importance of 1 (for the root node)
        }

        private double CalculateGlobalImportanceRecursive(App.Node node, double parentGlobalImportance)
        {
            // If node is null, which should not happen unless called incorrectly, return 0
            if (node == null)
                return 0;

            // Find all parents of the current node. In a typical tree, a root node will have no parents.
            var parents = App.HierarchyData.Where(n => n.Children.Contains(node)).ToList();

            // If no parents are found, the node is the root.
            if (!parents.Any())
                return 1;  // The global importance of the root is defined as 1.

            double globalImportance = 0;

            foreach (var parent in parents)
            {
                int index = parent.Children.IndexOf(node);
                double localImportance = parent.LocalPriorities[index]; // Local importance of this node with respect to the current parent

                // Recursive call to compute the global importance of the parent
                double parentGlobal = CalculateGlobalImportanceRecursive(parent, parentGlobalImportance);
                globalImportance += localImportance * parentGlobal;
                //sum product of localimportance of the child with its all parents
            }

            return globalImportance;
        }

        private async void SendEmail(List<DecisionGlobalRating> finalResult)
        {
            // Prepare the list for the email
            var emailFormattedResults = finalResult.Select(r => new 
            {
                Decision = r.DecisionName,
                GlobalPriority = $"{Math.Round(r.GlobalImportance * 100, 2)}%" // Convert for email in anonymous object
            }).ToList();

            string[] recipients =
            {
                "grigory.pishchulov@manchester.ac.uk"
            };
            string subject = $"AHP Results for {Title}"; //overall goal in the subject
            string body = JsonConvert.SerializeObject(emailFormattedResults,
                Formatting.Indented);

            EmailMessage message;
            try
            {
                message = new EmailMessage(subject, body, recipients);
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        
		public class DecisionGlobalRating
		{
			public string DecisionName { get; set; }
			public double GlobalImportance { get; set; }
		}

        private void SubmitBtn_OnClicked(object sender, EventArgs e)
        {
            var result = DecisionRatingList.ItemsSource.Cast<DecisionGlobalRating>().ToList();
            SendEmail(result);
        }
    }
}

