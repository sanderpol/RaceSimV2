
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }
        public static event EventHandler<NextRaceEventArgs> NextRaceEvent;

        public static void Initialize(Competition competition)
        {
            Competition = competition;
            AddParticipants();
            AddTracks();
        }

        public static void NextRace()
        {
            Track nextTrack = Competition.NextTrack();
            if (nextTrack != null)
            {
                CurrentRace = new Race(nextTrack, Competition.Participants);
                CurrentRace.SetStartGrid(Competition.Participants);
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs() { Race = CurrentRace });
                CurrentRace.StartRace();
            }
        }




        public static void AddParticipants()
        {
            var maxVerstappen = new Driver("Max Verstappen", 0);
            maxVerstappen.Equipment = new Bolide();
            maxVerstappen.TeamColor = TeamColors.RED;
            Competition.Participants.Add(maxVerstappen);
            var sebastiaanVettel = new Driver("Sebastiaan Vettel", 0);
            sebastiaanVettel.Equipment = new Bolide();
            sebastiaanVettel.TeamColor = TeamColors.GREEN;
            Competition.Participants.Add(sebastiaanVettel);
            var landoNorris = new Driver("Lando Norris", 0);
            landoNorris.Equipment = new Bolide();
            landoNorris.TeamColor = TeamColors.BLUE;
            Competition.Participants.Add(landoNorris);
            var charlesLegreg = new Driver("Charles Legreg", 0);
            charlesLegreg.Equipment = new Bolide();
            charlesLegreg.TeamColor = TeamColors.YELLOW;
            Competition.Participants.Add(charlesLegreg);
        }

        public static void AddTracks()
        {
            var simple = new Track("Circle", 0, 3,
                new SectionTypes[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner
                });
            var elburg = new Track("Circuit Elburg", 0, 3,
                new SectionTypes[]
                {
                    SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner,
                    SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner,
                    SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.StartGrid
                });
            var oostendorp = new Track("Oostendorp", 0, 2,
                new SectionTypes[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish,
                    SectionTypes.Straight,
                    SectionTypes.LeftCorner, SectionTypes.Straight, 
                    SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.LeftCorner, 
                    SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.LeftCorner,
                    SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight
                });
            var figure0 = new Track("Figure_0", 0, 5,
                new[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.RightCorner
                });

            Competition.Tracks.Enqueue(simple);
            Competition.Tracks.Enqueue(elburg);
            Competition.Tracks.Enqueue(oostendorp);
            Competition.Tracks.Enqueue(figure0);
        }
    }
}