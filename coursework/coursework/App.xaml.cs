using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace coursework
{
    public partial class App : Application
    {
        public static List<Node> HierarchyData { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            LoadHierarchyDataAsync();
        }

        private async void LoadHierarchyDataAsync()
        {
            try
            {
                HierarchyData = await FetchHierarchyAsync();
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await MainPage.DisplayAlert("Error", e.Message, "OK");
                });
            }
        }


        private async Task<List<Node>> FetchHierarchyAsync()
        {
            using (var client = new HttpClient())
            {
                string url = "https://personalpages.manchester.ac.uk/staff/grigory.pishchulov/Hierarchy.json";
                var json = await client.GetStringAsync(url);
                var flatNodes = JsonConvert.DeserializeObject<List<TempNode>>(json);

                List<Node> nodes = new List<Node>();

                // Creating all nodes but with empty children list of Nodes 
                foreach (var tempNode in flatNodes)
                {
                    nodes.Add(new Node
                    {
                        Name = tempNode.Name,
                        Description = tempNode.Description,
                        Children = new List<Node>()
                    });
                }

                // linking children
                foreach (var node in nodes)
                {
                    var tempNode = flatNodes.Find(tn => tn.Name == node.Name);
                    if (tempNode != null && tempNode.Children != null) // Check if tempNode is not null
                    {
                        foreach (var childName in tempNode.Children)
                        {
                            var childNode = nodes.Find(n => n.Name == childName);
                            if (childNode != null)
                            {
                                node.Children.Add(childNode);
                            }
                        }
                    }
                    else
                    {
                        // Handle the case where no corresponding TempNode was found
                        Console.WriteLine($"Warning: No data found for node '{node.Name}'.");
                    }
                }
                return nodes; // return the list of nodes
            }
        }
        //this approach was used to not do .Find by name of the child and iterate through the List Hierarchydata to find the child node
        public class Node
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Node> Children { get; set; } = new List<Node>();
            public List<double> LocalPriorities { get; set; } = new List<double>();
        }

        public class TempNode
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public List<string> Children { get; set; }
        }
    }
}
