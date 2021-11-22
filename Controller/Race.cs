using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Race
    {
        public Track Track { get; set; }
        public List<IParticipant> Participants { get; set; }
        public DateTime StartTime { get; set; }
        public Random Random { get; set; }


        private Dictionary<Section, SectionData> Positions;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            Positions = new Dictionary<Section, SectionData>();
            Random = new Random(DateTime.Now.Millisecond);
            RandomizeEquipment();
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
