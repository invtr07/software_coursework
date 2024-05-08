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
        private void AssignLocalPriorities(App.Node parent, double[] rowTotals)
        {
            if (parent.LocalPriorities == null)
                parent.LocalPriorities = new List<double>();
            
            int n = rowTotals.Length;
            double sumOfRowTotals = rowTotals.Sum(); // Sum all elements for normalization to 1

            parent.LocalPriorities.Clear();

            for (int i = 0; i < n; i++)
            {
                
                parent.LocalPriorities.Add(rowTotals[i] / sumOfRowTotals); // Normalize each priority
                // parent.LocalPriorities[i]= rowTotals[i] / sumOfRowTotals; // Normalize each priority
            }
        }

        public void CalculateLocalPriorities(List<ChildLocalComparison> comparisons, App.Node parent)
        {
            double[,] matrix = GenerateComparisonMatrix(comparisons, parent.Children);
            matrix = NormalizeMatrix(matrix);
            double[] rowTotals = CalculateRowTotals(matrix);
            
            AssignLocalPriorities(parent, rowTotals);
        }
        

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (_currentNode == null)
                {
                    await DisplayAlert("Error", "Current node is undefined.", "OK");
                    return;
                }

                if (_currentNode.Children == null || !_currentNode.Children.Any())
                {
                    await DisplayAlert("Error", "No children nodes available for evaluation.", "OK");
                    return;
                }

                var comparisons = BindingContext as List<ChildLocalComparison>;
                if (comparisons == null)
                {
                    await DisplayAlert("Error", "Comparison data is missing or corrupt.", "OK");
                    return;
                }

                CalculateLocalPriorities(comparisons, _currentNode);

                // Update the main hierarchy data stored in the App class
                // UpdateAppHierarchyData(_currentNode);

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
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