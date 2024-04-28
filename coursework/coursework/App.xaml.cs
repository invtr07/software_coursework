using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace coursework
{
    public partial class App : Application
    {
        public static DataModels.AHPItem[] ahpJson;

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }


        protected override void OnStart ()
        {
            string s = "[]";
            WebRequest request = WebRequest.Create(
                "https://personalpages.manchester.ac.uk/staff/grigory.pishchulov/Hierarchy.json");
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                s = reader.ReadToEnd();
            }
            ahpJson = JsonConvert.DeserializeObject<DataModels.AHPItem[]>(s);
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}



