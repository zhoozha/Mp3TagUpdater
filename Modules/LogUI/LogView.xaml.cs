﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Practices.Unity;
using Mp3.Infrastructure;

namespace LogUI
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public LogView(IUnityContainer container)
        {
            InitializeComponent();
            this.ViewModel = container.Resolve<LogViewModel>();
        }

        [Import]
        LogViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }
    }
}
