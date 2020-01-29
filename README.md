## SettingsFile Plugin for Xamarin

Simple cross platform plugin to save settings in a configuration file.


### Setup
* Available on NuGet: https://www.nuget.org/packages/Plugin.SettingsFile [![NuGet](https://img.shields.io/nuget/v/Plugin.SettingsFile.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.SettingsFile/)
* Install into your PCL/.NET Standard project and Client projects.

**Platform Support**

|Platform|Version|
| ------------------- | :-----------: |
|Xamarin.iOS|iOS 10+|
|Xamarin.Android|API 19+|
|Windows 10 UWP|10+|

*See platform notes below

Build Status: [![Build status](https://raraavis.visualstudio.com/Xamarin.Plugins/_apis/build/status/Xamarin.Plugins-.NET%20Desktop-CI)](https://raraavis.visualstudio.com/Xamarin.Plugins/_build/latest?definitionId=10)

### Visual Studio configuration

It's necesary to add the configuration file to specific folder and configure it using properties tab.

![Android](https://github.com/josuuribe/Plugin.SettingsFile/blob/gh-pages/images/2018-07-15-xamarin-config-android.png)
![IOS](https://github.com/josuuribe/Plugin.SettingsFile/blob/gh-pages/images/2018-07-15-xamarin-config-ios.png)
![UWP](https://github.com/josuuribe/Plugin.SettingsFile/blob/gh-pages/images/2018-07-15-xamarin-config-uwp.png)

### Android Current Activity Setup

This plugin uses the [Current Activity Plugin](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md) to get access to the current Android Activity. Be sure to complete the full setup if a MainApplication.cs file was not automatically added to your application. Please fully read through the [Current Activity Plugin Documentation](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md). At an absolute minimum you must set the following in your Activity's OnCreate method:

```csharp
Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
```

It is highly recommended that you use a custom Application that are outlined in the Current Activity Plugin Documentation](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md)


### API Usage

You are able to load and get values with just a few lines of code, the file parameter is optional (default is 'config.json') and also CancellationToken object (by default None).

Load file: 
```csharp
AppSettings configuration = await CrossSettingsFile<AppSettings>.Current.LoadAsync("settings.json", cancellationToken);
```

Get configuration:
```csharp
AppSettings configuration = CrossSettingsFile<AppSettings>.Current.Get();
```

#### Contributors
Main code thanks to [John Thiriet](https://johnthiriet.com/) and [James Montemagno](https://montemagno.com/)

Thanks!

#### License
Licensed under main repo license(MIT)
