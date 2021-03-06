﻿//#define ENV_LOCAL
//#define ENV_DEV
//#define ENV_STAGE
#define ENV_MASTER

using LagoVista.Client.Core;
using LagoVista.Client.Core.Models;
using LagoVista.Client.Core.ViewModels;
using LagoVista.Core.Interfaces;
using LagoVista.Core.IOC;
using LagoVista.Core.ViewModels;
using LagoVista.Simulator.Core.ViewModels;
using LagoVista.Simulator.Core.ViewModels.Messages;
using LagoVista.Simulator.Core.ViewModels.Simulator;
using LagoVista.XPlat.Core.Services;

using Xamarin.Forms;
using System;
using LagoVista.XPlat.Core.Views;
using LagoVista.Core.PlatformSupport;
using System.Resources;
using LagoVista.MQTT.Core.Clients;
using LagoVista.Core.Networking.Interfaces;
using LagoVista.Core.Models;

namespace LagoVista.Simulator
{
    public partial class App : Application
    {
        static App _instance;
        AppConfig _appConfig;

        public App()
        {
            InitializeComponent();

            _instance = this;

           InitServices();
        }

        private void InitServices()
        {
            _appConfig = new AppConfig();

#if ENV_MASTER
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "api.nuviot.com",
            };
            _appConfig.Environment = Environments.Production;
#elif ENV_DEV
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "dev-api.nuviot.com",
            };
            _appConfig.Environment = Environments.Development;
#elif ENV_LOCAL
            var serverInfo = new ServerInfo()
            {
                SSL = false,
                RootUrl = "localhost:5001",
            };
            _appConfig.Environment = Environments.Local;
#elif ENV_STAGE
            var serverInfo = new ServerInfo()
            {
                SSL = true,
                RootUrl = "stage-api.nuviot.com",
            };
            _appConfig.Environment = Environments.Staging;
#endif

            var clientAppInfo = new ClientAppInfo()
            {
                MainViewModel = typeof(MainViewModel)
            };

            DeviceInfo.Register();

            var deviceInfo = SLWIOC.Get<IDeviceInfo>();

            SLWIOC.RegisterSingleton<IClientAppInfo>(clientAppInfo);
            SLWIOC.RegisterSingleton<IAppConfig>(_appConfig);

            var navigation = new ViewModelNavigation(this);
            LagoVista.XPlat.Core.Startup.Init(this, navigation);
            LagoVista.Client.Core.Startup.Init(serverInfo);

            navigation.Add<MainViewModel, Views.MainView>();
            navigation.Add<SimulatorViewModel, Views.Simulator.SimulatorView>();
            navigation.Add<SimulatorEditorViewModel, Views.Simulator.SimulatorEditorView>();
            navigation.Add<MessageEditorViewModel, Views.Messages.MessageEditorView>();
            navigation.Add<SendMessageViewModel, Views.Messages.SendMessageView>();
            navigation.Add<MessageHeaderViewModel, Views.Messages.MessageHeaderView>();
            navigation.Add<PasswordEntryViewModel, Views.Simulator.PasswordEntryView>();
            navigation.Add<UnlockStorageViewModel, Views.Simulator.UnlockStorageView>();
            navigation.Add<SetStoragePasswordViewModel, Views.Simulator.SetStoragePasswordView>();
            navigation.Add<DynamicAttributeViewModel, Views.Messages.DynamicAttributeView>();

            navigation.Add<SplashViewModel, Views.SplashView>();

            navigation.Start<SplashViewModel>();

            SLWIOC.Register<IMQTTAppClient, MQTTAppClient>();
            SLWIOC.Register<IMQTTDeviceClient, MQTTDeviceClient>();

            SLWIOC.RegisterSingleton<IViewModelNavigation>(navigation);
        }

        public static App Instance { get { return _instance; } }
        
        public void HandleURIActivation(Uri uri)
        {
            var logger = SLWIOC.Get<ILogger>();
            if (this.MainPage == null)
            {
                logger.AddCustomEvent(LogLevel.Error, "App_HandleURIActivation", "Main Page Null");
            }
            else
            {
                if (this.MainPage is LagoVistaNavigationPage page)
                {
                    page.HandleURIActivation(uri);
                }
                else
                {

                    logger.AddCustomEvent(LogLevel.Error, "App_HandleURIActivation", "InvalidPageType - Not LagoVistaNavigationPage", new System.Collections.Generic.KeyValuePair<string, string>("type", this.MainPage.GetType().Name));
                }
            }

        }

        public void SetVersion(VersionInfo info)
        {
            _appConfig.Version = info;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
