
using Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        
        public DateTime StartTime { get; set; }
        public System.Timers.Timer Timer { get; set; } = new System.Timers.Timer(50);
        public Random Random { get; set; }


        private Dictionary<Section, SectionData> Positions;
        

        //FinishData
        public Dictionary<IParticipant, int> LapsDriven { get; set; }
        public Dictionary<IParticipant, int> SectionDriven { get; set; }
        public int TotalFinishers { get; set; }
        public bool FinshFlag { get; set; }
        public bool RaceFinished { get; set; }

        public List<DriverRaceTimer> DriverTimers { get; set; }



        //Event
        public event EventHandler RaceFinishedEvent;
        public event EventHandler<DriversChangedEventArgs> DriverChanged;
        public event EventHandler<DriverMovedEventArgs> DriverMoved;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            Positions = new Dictionary<Section, SectionData>();
            Random = new Random(DateTime.Now.Millisecond);
            RandomizeEquipment();
            CleanUp();

            Timer.Elapsed += OnTimedEvent;
        }

        private void CleanUp()
        {
            RaceFinished = false;
            FinshFlag = false;
            LapsDriven = new Dictionary<IParticipant, int>();
            SectionDriven = new Dictionary<IParticipant, int> { };
            DriverTimers = new List<DriverRaceTimer>();
            Participants.ForEach(item => LapsDriven.Add(item, 0));
            Participants.ForEach(item => SectionDriven.Add(item, 0));
            Participants.ForEach(item => DriverTimers.Add(new DriverRaceTimer()
            {
                Driver = item,
                LapTimer = new Stopwatch(),
                TotalRaceTimer = new Stopwatch()
            }));
            TotalFinishers = 0;
        }


        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            CheckDriverFinished();
            MoveDrivers();
            RandomizeBreakOrRepair();

            DriverChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track, DriverTimers = this.DriverTimers });
            if (RaceFinished) Console.WriteLine(RaceFinished);
            RaceFinishedEvent?.Invoke(this, new EventArgs());
        }

        //TODO: Write test
        private void CheckDriverFinished()
        {
            if (TotalFinishers == Participants.Count) RaceFinished = true;
            if (LapsDriven.Any(item => item.Value == Track.TotalLaps)) FinshFlag = true;
            if(LapsDriven.All(item => item.Value > Track.TotalLaps)) RaceFinished = true;
        }

        //TODO: Write test
        private void RandomizeBreakOrRepair()
        {
            const int constChance = 10;
            foreach (var particpant in Participants){
                if (particpant.Equipment.IsBroken)
                {
                    if(Random.Next(0,5) == 0)
                    {
                        particpant.Equipment.IsBroken = false;
                        particpant.Equipment.Quality = Random.Next(45, 61);
                    }
                }
                else
                {
                    var chance = constChance + particpant.Equipment.Quality;
                    if (Random.Next(0, chance) == chance - 1)
                    {
                        particpant.Equipment.IsBroken = true;
                    }
                    else
                    {
                        if (particpant.Equipment.Quality >= constChance) { 
                        particpant.Equipment.Quality -= 4;
                        }
                    }
                }
            }
        }

        public SectionData GetSectionData(Section section)
        {
            var data = Positions.GetValueOrDefault(section);
            if(data == null)
            {
                data = new SectionData();
                Positions.Add(section, data);
            }
            return data;
        }

        public void StartRace()
        {
            Timer.Enabled = true;
            StartTime = DateTime.Now;
            DriverTimers.ForEach(driverTimer =>
            {
                driverTimer.TotalRaceTimer.Start();
                driverTimer.LapTimer.Start();
            });

        }
        public void SetStartGrid(List<IParticipant> participants)
        {
            var startGridLuckNumber = new int[participants.Count];
            var startPositions = new List<IParticipant>();

            for (var i = 0; i < startGridLuckNumber.Length; i++)
            {
                startGridLuckNumber[i] = Random.Next(0, 10000);
            }

            for (var i = 0; i < startGridLuckNumber.Length; i++)
            {
                var added = false;
                for (var j = 0; j < startGridLuckNumber.Length; j++)
                {
                    if (startGridLuckNumber[i] <= startGridLuckNumber[j] || added != false) continue;
                    if (j > startPositions.Count) continue;
                    startPositions.Insert(j, participants[i]);
                    added = true;
                }

                if (added == false)
                {
                    startPositions.Add(participants[i]);
                }
            }

            var startGrids = Track.Sections.Where(sec => sec.SectionType.Equals(SectionTypes.StartGrid)).ToList();
            var startRow = 1;
            var side = false;

            for (var i = startPositions.Count; i > 0; i--)
            {
                SetSectionParticipant(participants[i-1], side, startGrids[startRow]);
                side = !side;
                if (i % 2 == 1)
                    startRow++;
            }
        }

        private void SetSectionParticipant(IParticipant participant, bool side, Section section)
        {
            var sectionData = GetSectionData(section);

            if (side)
                sectionData.Left = participant;
            else
                sectionData.Right = participant;

            SectionDriven[participant]++;
            var data = GetDriverDriven(participant);
            DriverMoved?.Invoke(this, new DriverMovedEventArgs { Track = this.Track, DriverMoved = data });

        }

        public void MoveDrivers()
        {
            LinkedListNode<Section> sectionNodes = Track.Sections.Last;

            
            while (sectionNodes != null)
            {                
                var currentPos = GetSectionData(sectionNodes.Value);
                var targetSection = sectionNodes.Next != null ? sectionNodes.Next.Value : Track.Sections.First?.Value;
                var targetPos = GetSectionData(targetSection);


                if (currentPos.Left != null && !currentPos.Left.Equipment.IsBroken)
                {
                    currentPos.DistanceLeft = SetSectionDistance(currentPos.Left, currentPos.DistanceLeft);
                }

                if (currentPos.DistanceLeft >= Section.SectionLength)
                {
                    var overlap = currentPos.DistanceLeft - Section.SectionLength;
                    MoveDriver(sectionNodes.Value, currentPos, targetSection, targetPos, overlap, true);
                }

                if (currentPos.Right != null && !currentPos.Right.Equipment.IsBroken)
                {
                    currentPos.DistanceRight = SetSectionDistance(currentPos.Right, currentPos.DistanceRight);
                }

                if (currentPos.DistanceRight >= Section.SectionLength)
                {
                    var overlap = currentPos.DistanceRight - Section.SectionLength;
                    MoveDriver(sectionNodes.Value, currentPos, targetSection, targetPos, overlap, false);
                }
                

                sectionNodes = sectionNodes.Previous;
            } 
        }

        public void MoveDriver(Section currentSection, SectionData currentPos, Section targetSection,
            SectionData targetPos, int overlap, bool isLeft)
        {
            bool isMoved = false;
            var Next = Random.Next(0, 2);
            if (Next == 0)
            {
                if (targetPos.Left != null)
                {
                    if (isLeft)
                        currentPos.DistanceLeft = Section.SectionLength - 1;
                    else
                        currentPos.DistanceRight = Section.SectionLength - 1;
                }
                else
                {
                    
                    SetSectionParticipant(isLeft ? currentPos.Left : currentPos.Right, true, targetSection );
                    targetPos.DistanceLeft = overlap;
                    isMoved = true;
                }
                if (currentSection.SectionType == SectionTypes.Finish && currentPos.Left != null)
                {
                    AddLapToDriver(currentPos.Left);
                }

            }
            else
            {
                if (targetPos.Right != null)
                {
                    if (isLeft)
                        currentPos.DistanceLeft = Section.SectionLength - 1;
                    else
                        currentPos.DistanceRight = Section.SectionLength - 1;
                }
                else
                {                   
                    SetSectionParticipant(isLeft ? currentPos.Left : currentPos.Right, false, targetSection);
                    targetPos.DistanceRight = overlap;
                    

                    isMoved = true;
                }
                if (currentSection.SectionType == SectionTypes.Finish && currentPos.Right != null)
                {
                    AddLapToDriver(currentPos.Right);
                }
            }

            if (!isMoved) return;
            
            

            if (isLeft)
            {
                Positions[currentSection].DistanceLeft = 1;
                Positions[currentSection].Left = null;
            }
            else
            {
                Positions[currentSection].DistanceRight = 1;
                Positions[currentSection].Right = null;
            }
        }

        public DriverDriven GetDriverDriven(IParticipant participant)
        {
            return new DriverDriven()
            {
                Participant = participant,
                lapCount = LapsDriven[participant],
                sectionCount = SectionDriven[participant]
            };
        }

        private int SetSectionDistance(IParticipant currentParticipant, int currentDistance)
        {
            return currentDistance + (currentParticipant.Equipment.Performance * currentParticipant.Equipment.Speed);
        }


        public void AddLapToDriver(IParticipant driver)
        {
            if (FinshFlag)
            {
                TotalFinishers += 1;
            }
            LapsDriven[driver] += 1;
            SectionDriven[driver] = 0;
            DriverTimers.Where(item => item.Driver == driver).ToList().ForEach(item => item.LapTimer.Restart());
            

        }

        /// <summary>
        /// Randomize the equipment foreach of the participants
        /// </summary>
        public void RandomizeEquipment()
        {
            Participants.ForEach(p =>p.Equipment.Speed = Random.Next(8, 10));
            Participants.ForEach(p => p.Equipment.Quality = Random.Next(85, 101));
            Participants.ForEach(p => p.Equipment.Performance = Random.Next(8, 10));
        }
    }
    
    public class DriverRaceTimer
    {
        public IParticipant Driver { get; set; }
        public Stopwatch LapTimer { get; set; }
        public Stopwatch TotalRaceTimer { get; set; }
    }

    public class DriverDriven
    {
        public IParticipant Participant { get; set; }
        public int lapCount {get; set; }
        public double sectionCount { get; set; }

    }
}
