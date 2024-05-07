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
        public NodesEvaluation(App.Node node = null)
        {
            InitializeComponent();
            
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
                        Preference = 1, // Default neutral preference
                        Toggled = false
                    });
                }
            }
            return comparisons;
        }
        
        public class ChildLocalComparison
        {
            public App.Node Child1 { get; set; }
            public App.Node Child2 { get; set; }
            public int Preference { get; set; } // Preference scale from 1 to 9
            public bool Toggled { get; set; } // Direction of preference (Child1 > Child2 or Child2 > Child1)
        }
    }
}