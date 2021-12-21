using Controller;
using Model;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Data.Initialize(new Competition());
            Data.NextRaceEvent += OnNextRace;
            Data.NextRace();
            Data.CurrentRace.DriverChanged += OnDriversChanged;
            
            
        }

        public void OnDriversChanged(object sender, EventArgs e)
        {
            DriversChangedEventArgs driverE = (DriversChangedEventArgs)e;
            this.TrackImg.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.TrackImg.Source = null;
                    this.TrackImg.Source = Visualization.DrawTrack(driverE.Track);
                }));
        }

        public void OnNextRace(object sender, EventArgs e)
        {
            Cache.EmptyCache();
            //Data.NextRace();
            //Visualization.init(Data.CurrentRace);
            
            
        }

    }
}
