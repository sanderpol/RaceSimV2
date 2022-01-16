using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class RaceContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Race CurrentRace { get; set; }
        public List<IParticipant> Participants { get; set; }

        public void OnNextRace(object sender, NextRaceEventArgs e)
        {
            CurrentRace = e.Race;
            e.Race.DriverChanged += OndriversChanged;
        }

        public void OndriversChanged (object sender, DriversChangedEventArgs e)
        {
            Participants = CurrentRace.Participants;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

    }
}
