using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace NHapiSampleApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            this.Exit += this.Application_Exit;

            InitializeComponent();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
    }
}
