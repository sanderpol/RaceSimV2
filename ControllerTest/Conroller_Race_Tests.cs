using Controller;
using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTest
{
    [TestFixture]
    public class Conroller_Race_Tests
    {
        public static Race CurrentRace { get; set; }
        public Competition Competition { get; set; }

        public Track Figure0 = new Track("Figure_0", 0, 5,
                new[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner
                });


        [SetUp]
        public void Controller_Race_SetupCurrentRace()
        {
            var participants = Data.AddParticipants();
            CurrentRace = new Race(Figure0, participants);
            CurrentRace.SetStartGrid(CurrentRace.Participants);
        }

        [Test]
        public void Controller_Race_SetStartGrid()
        {

            var startGridSectors = CurrentRace.Track.Sections.Where(section => section.SectionType == SectionTypes.StartGrid).Reverse().ToList();
            int totalStarters = 0;
            bool restShouldBeEmpty = false;
            bool restIsEmpty = true;

            startGridSectors.ForEach(section =>
            {
                var data = CurrentRace.GetSectionData(section);
                if (data.Left != null)
                {
                    totalStarters++;
                    if (restShouldBeEmpty)
                    {
                        restIsEmpty = false;
                    }
                }
                if (data.Right != null)
                {
                    totalStarters++;
                    if (restShouldBeEmpty)
                    {
                        restIsEmpty = false;
                    }
                }
                else
                {
                    restShouldBeEmpty = true;
                }
            });
            Assert.IsTrue(restIsEmpty);
        }

        [Test]
        [Retry(4)]
        public void Controller_Race_MoveDriver()
        {
            var firstStartGrid = CurrentRace.Track.Sections.Where(section => section.SectionType == SectionTypes.StartGrid).Reverse().First();
            var finishGrid = CurrentRace.Track.Sections.Where(item => item.SectionType.Equals(SectionTypes.Finish)).First();


            var startData = CurrentRace.GetSectionData(firstStartGrid);
            startData.DistanceLeft = Section.SectionLength + 1;
            startData.DistanceRight = Section.SectionLength + 1;
            CurrentRace.MoveDriver(firstStartGrid, CurrentRace.GetSectionData(firstStartGrid), finishGrid, CurrentRace.GetSectionData(finishGrid), 0, true);
            CurrentRace.MoveDriver(firstStartGrid, CurrentRace.GetSectionData(firstStartGrid), finishGrid, CurrentRace.GetSectionData(finishGrid), 0, false);

            var finishData = CurrentRace.GetSectionData(finishGrid);
            bool isMoved = false;
            if (finishData.Left != null && finishData.Right != null)
            {
                isMoved = true;

            }
            else if ((finishData.Left != null && finishData.Right == null) || (finishData.Left == null && finishData.Right != null))
            {
                if (startData.DistanceLeft == Section.SectionLength - 1 || startData.DistanceRight == Section.SectionLength - 1)
                {
                    isMoved = true;
                }
            }

            Assert.IsTrue(isMoved);
        }

        [Test]
        public void Controller_Race_RandomizeEquipment()
        {
            CurrentRace.RandomizeEquipment();
            bool isNotRandom = false;
            CurrentRace.Participants.ForEach(particpant =>
            {
                if (particpant.Equipment.Quality == 0 || particpant.Equipment.Speed == 0)
                {
                    isNotRandom = true;
                }
            });
            Assert.IsFalse(isNotRandom);

        }


        [Test]
        [TestCase(false)]
        [TestCase(true)]
        public void Controller_Race_AddlapToDriver(bool isFinished)
        {

            CurrentRace.FinshFlag = isFinished;

            CurrentRace.Participants.ForEach(particpant =>
            {
                CurrentRace.AddLapToDriver(particpant);
                Assert.AreEqual(CurrentRace.LapsDriven[particpant], 1);
            });
            if (isFinished)
            {
                Assert.AreEqual(CurrentRace.Participants.Count, CurrentRace.TotalFinishers);
            }
            
        }





    }
}
