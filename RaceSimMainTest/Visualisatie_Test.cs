using Controller;
using Model;
using NUnit.Framework;
using System.Collections.Generic;

namespace RaceSimMainTest
{
    public class Tests
    {
        public Track TestTrack { get; set; }
        public string[][] ExpectedSectionStringArrays;
        public SectionData TestSectionData { get; set; }

        [SetUp]
        public void Setup()
        {
            TestTrack = new Track("StartGrid", 1, 1, new SectionTypes[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid
                });
            ExpectedSectionStringArrays = new string[][]
            {
                new string[]{"|      |","|MV    |","|      |","|    SV|","|      |"},
                new string[]{"|      |","|LN    |","|      |","|    CL|","|      |"},
                new string[]{"|      |","|      |","|      |","|      |","|      |"}
            };

            //TestSectionData Setup

            TestSectionData = new SectionData();
            TestSectionData.Left = new Driver("Max Verstappen", 0);
            TestSectionData.Right = new Driver("Sebastiaan Vettel", 0);
            TestSectionData.Left.Equipment = new Bolide();
            TestSectionData.Right.Equipment = new Bolide();
        }

        [Test]
        public void ReplaceTextWithDrivers()
        {
            var sections = TestTrack.Sections;

            var resultStrings1 = Visualisatie.SetSectionString(Visualisatie.GetSingleSectionStringArray(0, sections.First.Value.SectionType), "Max Verstappen", "Sebastiaan Vettel");
            var resultStrings2 = Visualisatie.SetSectionString(Visualisatie.GetSingleSectionStringArray(0, sections.First.Value.SectionType), "Lando Norris", "Charles Leclerc");

            Assert.AreEqual(ExpectedSectionStringArrays[0], resultStrings1);
            Assert.AreEqual(ExpectedSectionStringArrays[1], resultStrings2);


        }
        [Test]
        public void Visualisatie_ExpectEmptyString()
        {
            var result = Visualisatie.SetSectionString(Visualisatie.GetSingleSectionStringArray(0, SectionTypes.Straight), null, null);
            Assert.AreEqual(ExpectedSectionStringArrays[2], result);
        }

        [TestCase(0, 1)]
        [TestCase(1, 2)]
        [TestCase(2, 3)]
        [TestCase(3, 0)]
        public void ChangeDirectionRight_Input_ExpectedOutput(int input, int expected)
        {
            // Act
            var result = Visualisatie.SetNew_direction(SectionTypes.RightCorner, input);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestCase(1, 0)]
        [TestCase(2, 1)]
        [TestCase(3, 2)]
        [TestCase(0, 3)]
        public void ChangeDirectionLeft_Input_ExpectedOutput(int input, int expected)
        {
            // Act
            var result = Visualisatie.SetNew_direction(SectionTypes.LeftCorner, input);

            // Assert
            Assert.AreEqual(expected, result);
        }

        private static object[] Tracks =
{
            new object[] { new Track("Circle", 0, 3,
                new SectionTypes[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.LeftCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.LeftCorner
                }), 40, 20, 32, 26
            }
        };



        [Test]
        [TestCaseSource(nameof(Tracks))]
        public void Visualisation_Setup_TestMaxXY(Track track, int eMaxWidth, int eMaxHeight, int eCursorX, int eCursorY)
        {
            Visualisatie.CalcMaxXY(track, out int maxWidth, out int maxHeight, out int cursorX, out int cursorY);
            Assert.AreEqual(eMaxWidth, maxWidth);
            Assert.AreEqual(eMaxHeight, maxHeight);
            Assert.AreEqual(eCursorX, cursorX);
            Assert.AreEqual(eCursorY, cursorY);
        }

        [Test]
        [TestCase(true, "Max Verstappen", false)]
        [TestCase(false, "Sebastiaan Vettel", false)]
        [TestCase(true, "xx", true)]
        [TestCase(false, "xx", true)]
        [TestCase(false, null, true)]
        [TestCase(true, null, true)]
        public void Visualisation_GetParticpantStringName(bool isLeft, string? expectedResult, bool broken)
        {
            if (expectedResult == null)
            {
                TestSectionData.Left = null;
                TestSectionData.Right = null;
            }
            else if (broken)
            {
                TestSectionData.Left.Equipment.IsBroken = true;
                TestSectionData.Right.Equipment.IsBroken = true;

            }
            else
            {
                TestSectionData.Left.Equipment.IsBroken = false;
                TestSectionData.Right.Equipment.IsBroken = false;
            }
            string result = Visualisatie.GetParticpantStringName(TestSectionData, isLeft);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [TestCase(0, 0, 5, 0, 0)]
        [TestCase(1, 0, 0, 8, 0)]
        [TestCase(2, 0, 0, 0, 5)]
        [TestCase(3, 8, 0, 0, 0)]
        public void Visualisation_SetNewCursorPos(int dir, int cX, int cY, int eX, int eY)
        {
            Visualisatie.SetNewCursorPos(dir, cX, cY, out cX, out cY);
            Assert.AreEqual(cX, eX);
            Assert.AreEqual(cY, eY);
        }

        private static object[] TrackCourses =
        {
            new object[] { Visualisatie.finishHor, 1, SectionTypes.Finish},
            new object[] { Visualisatie.finishVer, 2, SectionTypes.Finish},
            new object[] { Visualisatie.startHor, 1, SectionTypes.StartGrid},
            new object[] { Visualisatie.startVer, 2, SectionTypes.StartGrid},
            new object[] { Visualisatie.straightHor, 1, SectionTypes.Straight},
            new object[] { Visualisatie.straightVer, 2, SectionTypes.Straight},
            new object[] { Visualisatie.LeftTurnUp, 1, SectionTypes.LeftCorner},
            new object[] { Visualisatie.RightTurnUp, 2, SectionTypes.LeftCorner},
            new object[] { Visualisatie.RightTurnDown, 1, SectionTypes.RightCorner},
            new object[] { Visualisatie.LeftTurnUp, 2, SectionTypes.RightCorner},
        };

        [Test, TestCaseSource(nameof(TrackCourses))]
        public void Visualisation_GetSingleSectionStringArray(string[]expectedResult, int direction, SectionTypes type)
        {
            var result = Visualisatie.GetSingleSectionStringArray(direction, type);
            Assert.AreEqual(expectedResult, result);
        }


    }
}