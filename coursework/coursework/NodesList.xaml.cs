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
            if (e.SelectedItem == null)
                return;

            var selectedNode = (App.Node)e.SelectedItem;
            
            await Navigation.PushAsync(new NodesList(selectedNode));
            ((ListView)sender).SelectedItem = null;
        }

        private void Evaluate_Clicked(object sender, EventArgs e)
        {
            App.Node currentNode = (App.Node)BindingContext;
            
            Navigation.PushAsync(new NodesEvaluation(currentNode));
        }
    }
}