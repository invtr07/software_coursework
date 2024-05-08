using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace coursework
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodesEvaluation : ContentPage
    {
        private App.Node _currentNode;
        public NodesEvaluation(App.Node node = null)
        {
            InitializeComponent();
            _currentNode = node;
            
            this.Title = node.Name;
            
            List<ChildLocalComparison> comparisons = GenerateComparisons(node.Children);
            this.BindingContext = comparisons;
        }
        private List<ChildLocalComparison> GenerateComparisons(List<App.Node> children) 
        {
            // Generate all possible comparisons between children by iterating through columns and rows like structure
            var comparisons = new List<ChildLocalComparison>();
            for (int i = 0; i < children.Count - 1; i++)
            {
                for (int j = i + 1; j < children.Count; j++)
                {
                    comparisons.Add(new ChildLocalComparison
                    {
                        Child1 = children[i],
                        Child2 = children[j],
                        Preference = -1, // Default neutral preference
                        Toggled = false
                    });
                }
            }
            return comparisons;
        }
        private double[,] GenerateComparisonMatrix(List<ChildLocalComparison> comparisons, List<App.Node> children)
        {
            int n = children.Count;
            double[,] matrix = new double[n, n];

            //  diagonal is 1 since each element is equally important to itself
            for (int i = 0; i < n; i++)
            {
                matrix[i, i] = 1;
            }

            // Populating matrix based on comparisons
            foreach (var comparison in comparisons)
            {
                int index1 = children.IndexOf(comparison.Child1);
                int index2 = children.IndexOf(comparison.Child2);
                
                double value = comparison.Preference;

                if (comparison.Toggled == true)
                {
                    matrix[index1, index2] = 1.0 / value;
                    matrix[index2, index1] = value;
                }
                else
                {
                    matrix[index1, index2] = value;
                    matrix[index2, index1] = 1.0 / value;
                }
            }

            return matrix;
        }
        
        private double[,] NormalizeMatrix(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[] columnTotals = new double[n];

            // Calculate column totals
            for (int j = 0; j < n; j++)
            {
                for (int i = 0; i < n; i++)
                {
                    columnTotals[j] += matrix[i, j];
                }
            }

            // Normalize the matrix
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (columnTotals[j] != 0)  // Check to prevent division by zero
                        matrix[i, j] /= columnTotals[j];
                    else
                        matrix[i, j] = 0;
                }
            }

            return matrix;
        }
        
        private double[] CalculateRowTotals(double[,] matrix)
        {
            int n = matrix.GetLength(0);
            double[] rowTotals = new double[n];

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    rowTotals[i] += matrix[i, j];
                }
            }

            return rowTotals;
        }
        private void AssignLocalPriorities(List<App.Node> children, double[] rowTotals)
        {
            int n = rowTotals.Length;
            double sumOfRowTotals = rowTotals.Sum(); // Sum all elements for normalization to 1

            for (int i = 0; i < n; i++)
            {
                if (children[i].LocalPriorities == null)
                    children[i].LocalPriorities = new List<double>();

                children[i].LocalPriorities.Clear();
                children[i].LocalPriorities.Add(rowTotals[i] / sumOfRowTotals); // Normalize each priority
            }
        }

        public void CalculateLocalPriorities(List<ChildLocalComparison> comparisons, List<App.Node> children)
        {
            double[,] matrix = GenerateComparisonMatrix(comparisons, children);
            matrix = NormalizeMatrix(matrix);
            double[] rowTotals = CalculateRowTotals(matrix);
            AssignLocalPriorities(children, rowTotals);
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                List<ChildLocalComparison> comparisons = (List<ChildLocalComparison>)BindingContext;
                List<App.Node> children = _currentNode.Children;

                
                if (children == null || !children.Any())
                {
                    await DisplayAlert("Error", "No children nodes available for evaluation.", "OK");
                    return;
                }

                CalculateLocalPriorities(comparisons, children);

                // Building the message to display
                StringBuilder messageBuilder = new StringBuilder();
                foreach (var child in children)
                {
                    messageBuilder.AppendLine($"{child.Name}: {string.Join(", ", child.LocalPriorities.Select(p => p.ToString("N2")))}");
                }

                // Display the results in an alert
                await DisplayAlert("Local Priorities", messageBuilder.ToString(), "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        public class ChildLocalComparison
        {
            public App.Node Child1 { get; set; }
            public App.Node Child2 { get; set; }
            
            //used property for handling picker index conversion into the preference value
            private int _preference;
            public int Preference
            {
                get => _preference;
                set
                {
                    _preference = value + 1; 
                }
            } 
            public bool Toggled { get; set; } 
        }
    }
}