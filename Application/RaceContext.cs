using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class RaceContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Race CurrentRace { get; set; }
        public string TrackName { get => CurrentRace == null ? "" : "Track name : " + CurrentRace.Track.Name; }
        public List<IParticipant> Participants { get; set; }

        public ObservableCollection<DriverListView> DriverListViews { get; set; }

        public void OnNextRace(object sender, NextRaceEventArgs e)
        {
            CurrentRace = e.Race;
            e.Race.DriverChanged += OndriversChanged;
            e.Race.DriverMoved += OnDriverMoved;
            DriverListViews = new ObservableCollection<DriverListView>();
            CurrentRace.Participants.ForEach(item =>
            {
                DriverListViews.Add(new DriverListView()
                {
                    Driver = item
                });
            });
            OnPropertyChanged();
            OnPropertyChanged();


        }

        public void OndriversChanged (object sender, DriversChangedEventArgs e)
        {
            Participants = CurrentRace.Participants;
            e.DriverTimers.ForEach(timer =>
            {
                var view = DriverListViews.Where(item => item.Driver.Equals(timer.Driver)).First();
                view.RaceStopwatch = timer.TotalRaceTimer.Elapsed;
                view.LapStopwatch = timer.LapTimer.Elapsed;
            });

            OnPropertyChanged();
        }

        public void OnDriverMoved (object sender, DriverMovedEventArgs e)
        {
            var view = DriverListViews.Where(item => item.Driver.Equals(e.DriverMoved.Participant)).First();
            view.LapCount = e.DriverMoved.lapCount;
            var percentage =(int) (e.DriverMoved.sectionCount /((double) e.Track.Sections.Count) * 100);
            view.lapPercentage = percentage + "%";
            OnPropertyChanged();
        }
        protected virtual void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DriverListView
    {
        public IParticipant Driver { get; set; }
        public int Position { get; set; }
        public TimeSpan RaceStopwatch { get; set; }
        public int LapCount { get; set; }
        public TimeSpan LapStopwatch { get; set; }
        public string lapPercentage { get; set; }
        public int SectionCount { get; set; }
        public string ParticipantName { get => Driver.Name; }
        public TeamColors TeamColor { get => Driver.TeamColor; }
        public int ParticpantPoints { get => Driver.Points; }

        public static int GetDriverPosition(IParticipant driver)
        {
            //return RaceContext.DriverListViews.OrderByDescending(item => item.LapCount).ThenByDescending(item => item.SectionCount).ToList().FindIndex(item => item.Driver.Equals(driver)) + 1;
            return 0;
        }

    }
}
