﻿using LagoVista.XPlat.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LagoVista.Simulator.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashView : LagoVistaContentPage
    {
        public SplashView()
        {
            InitializeComponent();           
        }
    }  
}
