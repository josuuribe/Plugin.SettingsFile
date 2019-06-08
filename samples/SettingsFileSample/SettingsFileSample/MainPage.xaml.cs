using Plugin.SettingsFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SettingsFileSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var settings = await CrossSettingsFile.Current.GetConfiguration<AppSettings>();

            lUrl.Text = settings.BaseUrl;
        }
    }
}
