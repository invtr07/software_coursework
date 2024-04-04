using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace coursework
{
    public partial class App : Application
    {
        //class type for Goal and Criterion objects
        
        public class ParentNode { 
            public string Name;
            public string Description;
            public string[] Children;
        }

        //class type for Decision alternatives
        
        public class Node { 
            public string Name;
            public string Description;
            
        }

        //static array that is storing info objects about Goal
        public static ParentNode[] goal = new ParentNode[]
        {
            new ParentNode{
                Name="Prioritize IT projects",
                Description="Establish the overall priority of an IT project for the University.",
                Children= new string[] { "Strategic impact", "Financial impact", "Customer impact", "Project risk", "Feasibility" }
            },
        };

        public static ParentNode[] criteria = new ParentNode[]
        //static array of type ParentNode containing criteria objects
        {
            new ParentNode{
                Name="Strategic impact",
                Description="Impact in terms of alignment with the University's overall and IT strategies, customer intimacy and product/service excellence.",
                Children= new string[] { "100 Gbps Network Border", "Science De-militarized Zone (DMZ) Network", "Cloud-based Enterprise Resource Planning (ERP) Tool"} },
            new ParentNode{
                Name="Financial impact",
                Description="Impact in terms of operational and IT cost reduction and funding / cost recovery.",
                Children= new string[] { "100 Gbps Network Border", "Science De-militarized Zone (DMZ) Network", "Cloud-based Enterprise Resource Planning (ERP) Tool"}},

            new ParentNode{
                Name="Customer impact",
                Description="Impact on students, staff and research at the University.",
                Children= new string[] { "100 Gbps Network Border", "Science De-militarized Zone (DMZ) Network", "Cloud-based Enterprise Resource Planning (ERP) Tool" }},

            new ParentNode{
                Name="Project risk",
                Description="Project risk in terms of organisational and technical complexity.",
                Children= new string[] { "100 Gbps Network Border", "Science De-militarized Zone (DMZ) Network", "Cloud-based Enterprise Resource Planning (ERP) Tool" }},

            new ParentNode{
                Name="Feasibility",
                Description="Project feasibility in terms of alignment with architecture and capability.",
                Children= new string[] { "100 Gbps Network Border", "Science De-militarized Zone (DMZ) Network", "Cloud-based Enterprise Resource Planning (ERP) Tool" }}
        };

        public static Node[] alternatives = new Node[]
        {
            new Node{Name="100 Gbps Network Border" ,Description="The University currently has two (2) 10 Gbps connections to the border network. Increasingly, research grants show preference to universities or entities that have 100 Gbps capabilities to allow for improved collaboration and transfer of data between various research entities around the world." },

            new Node{ Name="Science De-militarized Zone (DMZ) Network", Description="The University currently passes all network traffic through network security hardware such as firewalls and intrusion prevention systems. This hardware can impede the flow of network traffic and slow transfers of large scientific research datasets. A science DMZ network removes some of these barriers allowing for large datasets to move more quickly and at scheduled times, minimizing the impact on other network traffic."},

            new Node{Name="", Description="The University currently uses several tools for enterprise resource planning. These tools have served the University well, but industry and product advancements have left the current system nearing end of life. A cloud-based ERP tool would move the University forward into a new set of tools to manage university resources." }

        };


        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

        }

    

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}

