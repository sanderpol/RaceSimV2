using Controller;
using Model;

namespace RaceSimMain
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Data.Initialize(new Competition());
            Data.NextRaceEvent += Visualisatie.OnNextRace;
            Data.NextRace();
            //Visualisatie.DrawTrack(Data.CurrentRace.Track);
            //Console.WriteLine(Data.CurrentRace.Track.Name);
            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}