using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

namespace coursework
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NodesList : ContentPage
    {
        
        public NodesList(App.Node node = null)
        {
            InitializeComponent();

            if (node == null)
            {
                var rootNode = App.HierarchyData.FirstOrDefault();
                this.Title = rootNode.Name;
                BindingContext = rootNode;
            }
            else
            {
                this.Title = node.Name;
                BindingContext = node;
            }
            EvaluateButton.IsVisible = node != null && node.Children.Count > 0;
        }

        private async void ListViewChildren_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                    return;

                var viewModel = (ChildViewModel)e.SelectedItem;
                
                if (viewModel == null)
                {
                    await DisplayAlert("Error", "Unexpected item type", "OK");
                    return;
                }
                
                var selectedNode = viewModel.Child;
            
                await Navigation.PushAsync(new NodesList(selectedNode));
                ((ListView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void LoadPriorities()
        {
            try
            {
                if (BindingContext is App.Node node)
                {
                    // Check if LocalPriorities is not null and has the same count as Children
                    if (node.LocalPriorities != null && node.LocalPriorities.Count == node.Children.Count)
                    {
                        var childViewModels = node.Children
                            .Select((child, index) => new ChildViewModel
                            {
                                Child = child,
                                LocalPriority = node.LocalPriorities[index]  // Safe to access index
                            })
                            .ToList();

                        ListViewChildren.ItemsSource = childViewModels;
                    }
                    else
                    {
                        // Handle the scenario where LocalPriorities are not yet available
                        var childViewModels = node.Children
                            .Select(child => new ChildViewModel
                            {
                                Child = child,
                                LocalPriority = 0 // Default or placeholder value
                            })
                            .ToList();

                        ListViewChildren.ItemsSource = childViewModels;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private void Evaluate_Clicked(object sender, EventArgs e)
        {
            App.Node currentNode = (App.Node)BindingContext;
            
            Navigation.PushAsync(new NodesEvaluation(currentNode));
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPriorities();
        }
        
        public class ChildViewModel
        {
            public App.Node Child { get; set; }
            public double LocalPriority { get; set; }
        }
    }
}