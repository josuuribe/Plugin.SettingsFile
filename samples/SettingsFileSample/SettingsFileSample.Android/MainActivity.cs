using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.CurrentActivity;
using Plugin.SettingsFile;

namespace SettingsFileSample.Droid
{
    [Activity(Label = "SettingsFileSample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            string url1 = (await CrossSettingsFile<AppSettings>.Current.LoadAsync()).BaseUrl;

            string url2 = CrossSettingsFile< AppSettings>.Current.Get().BaseUrl;

        }
    }
}