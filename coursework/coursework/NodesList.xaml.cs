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
                // Assuming the root is the first entry and there is at least one node in HierarchyData
                var rootNode = App.HierarchyData.FirstOrDefault();
                if (rootNode != null)
                {
                    this.Title = rootNode.Name;  // Set the title to the root node's name
                    BindingContext = rootNode;
                }
                else
                {
                    this.Title = "Root"; // Fallback title if no nodes are available
                }
            }
            else
            {
                this.Title = node.Name;  // Set the title to the current node's name
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
                await Navigation.PushAsync(new NodesList(selectedNode));
            }
            else
            {
                await Navigation.PushAsync(new NodesList(selectedNode));
                EvaluateButton.IsVisible = false;
            }

            // Deselect the item
            ((ListView)sender).SelectedItem = null;
        }

        private void Evaluate_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}