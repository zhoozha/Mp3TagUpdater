using System;
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
using Mp3.Infrastructure;
using Microsoft.Practices.Unity;

namespace MainUI
{
    /// <summary>
    /// Interaction logic for ProcessorView.xaml
    /// </summary>
    [ViewExport(RegionName = RegionNames.Main)]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public partial class ProcessorView : UserControl
    {
        public ProcessorView(IUnityContainer container)
        {
            InitializeComponent();
            this.ViewModel = container.Resolve<ProcessorViewModel>();
        }

        [Import]
        ProcessorViewModel ViewModel
        {
            set
            {
                this.DataContext = value;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
