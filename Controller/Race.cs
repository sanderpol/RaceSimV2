﻿using Model;
using System;
using System.Collections.Generic;
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
        private Dictionary<IParticipant, int> LapsDriven { get; set; }
        public bool RaceFinished { get; set; }



        //Event
        //public event EventHandler RaceFinishedEvent;
        public event EventHandler<DriversChangedEventArgs> DriverChanged;

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
            LapsDriven = new Dictionary<IParticipant, int>();
            Participants.ForEach(item => LapsDriven.Add(item, 1));

        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            //CheckDriverFinished();
            MoveDrivers(e.SignalTime);

            DriverChanged?.Invoke(this, new DriversChangedEventArgs() { Track = this.Track });
            //if (RaceFinished) RaceFinishedEvent?.Invoke(this, new EventArgs());
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
            var startRow = 0;
            var side = false;

            for (var i = 0; i < startPositions.Count; i++)
            {
                SetSectionParticipant(participants[i], side, startGrids[startRow]);
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
        }

        private void MoveDrivers(DateTime elapesdDateTime)
        {
            LinkedListNode<Section> sectionNodes = Track.Sections.Last;

            
            while (sectionNodes != null)
            {                
                var currentPos = GetSectionData(sectionNodes.Value);
                var targetSection = sectionNodes.Next != null ? sectionNodes.Next.Value : Track.Sections.First?.Value;
                var targetPos = GetSectionData(targetSection);


                if (currentPos.Left != null)
                {
                    currentPos.DistanceLeft = SetSectionDistance(currentPos.Left, currentPos.DistanceLeft);
                }

                if (currentPos.DistanceLeft >= 100)
                {
                    MoveDriver(sectionNodes.Value, currentPos, targetSection, targetPos, true);
                }

                if (currentPos.Right != null)
                {
                    currentPos.DistanceRight = SetSectionDistance(currentPos.Right, currentPos.DistanceRight);
                }

                if (currentPos.DistanceRight >= 100)
                {
                    MoveDriver(sectionNodes.Value, currentPos, targetSection, targetPos, false);
                }


                sectionNodes = sectionNodes.Previous;
            } 
        }

        private void MoveDriver(Section currentSection, SectionData currentPos, Section targetSection,
            SectionData targetPos, bool isLeft)
        {
            bool isMoved = false;
            var Next = Random.Next(0, 2);
            if (Next == 0)
            {
                if (targetPos.Left != null)
                {
                    if (isLeft)
                        currentPos.DistanceLeft = 99;
                    else
                        currentPos.DistanceRight = 99;
                }
                else
                {
                    if (currentSection.SectionType == SectionTypes.Finish && currentPos.Left != null)
                    {
                        AddLapToDriver(currentPos.Left);
                    }

                    SetSectionParticipant(isLeft ? currentPos.Left : currentPos.Right, true, targetSection);

                    isMoved = true;
                }
            }
            else
            {
                if (targetPos.Right != null)
                {
                    if (isLeft)
                        currentPos.DistanceLeft = 99;
                    else
                        currentPos.DistanceRight = 99;
                }
                else
                {
                    if (currentSection.SectionType == SectionTypes.Finish && currentPos.Right != null)
                    {
                        AddLapToDriver(currentPos.Right);
                    }

                    SetSectionParticipant(isLeft ? currentPos.Left : currentPos.Right, false, targetSection);

                    isMoved = true;
                }
            }

            if (!isMoved) return;
            if (isLeft)
            {
                Positions[currentSection].DistanceLeft = 0;
                Positions[currentSection].Left = null;
            }
            else
            {
                Positions[currentSection].DistanceRight = 0;
                Positions[currentSection].Right = null;
            }
        }

        private int SetSectionDistance(IParticipant currentParticipant, int currentDistance)
        {
            return currentDistance + (currentParticipant.Equipment.Performance * currentParticipant.Equipment.Speed);
        }


        private void AddLapToDriver(IParticipant driver)
        {
            LapsDriven[driver] += 1;
        }

        /// <summary>
        /// Randomize the equipment foreach of the participants
        /// </summary>
        private void RandomizeEquipment()
        {
            Participants.ForEach(p =>p.Equipment.Speed = Random.Next(8, 10));
            Participants.ForEach(p => p.Equipment.Quality = Random.Next(85, 101));
            Participants.ForEach(p => p.Equipment.Performance = Random.Next(8, 10));
        }
    }
}