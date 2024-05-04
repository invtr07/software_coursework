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
    public partial class NodesList : ContentPage
    {
        public NodesList(App.Node node = null)
        { 
            InitializeComponent();

            if (node == null)
            {
                // When no specific node is provided, assume we want the root level
                this.Title = node.Name;
                BindingContext = App.HierarchyData.FirstOrDefault(); // Assuming the root is the first entry
            }
            else
            {
                this.Title = node.Name;
                BindingContext = node;
            }
        }
        
        private async void ListViewChildren_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedNode = (App.Node)e.SelectedItem;
            if (selectedNode.Children.Count > 0)
            {
                // Navigate to a new NodesList page, passing the selected node as the context
                await Navigation.PushAsync(new NodesList { BindingContext = selectedNode });
            }
            else
            {
                // Optionally handle selection for nodes without children
                await DisplayAlert("Info", $"Node: {selectedNode.Name} has no further sub-nodes.", "OK");
            }

            // Deselect the item
            ((ListView)sender).SelectedItem = null;
        }

        private void Evaluate_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}