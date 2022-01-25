using Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class MainContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Race CurrentRace{ get; set; }
        public string TrackName { get => CurrentRace == null ? "" : "Track name : " + CurrentRace.Track.Name; }

        public void OnNextRace(object sender, NextRaceEventArgs e)
        {
            CurrentRace = e.Race;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

    }
}
