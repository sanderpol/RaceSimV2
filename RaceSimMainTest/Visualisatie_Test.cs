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

        [SetUp]
        public void Setup()
        {
            TestTrack = new Track("StartGrid", 1,1, new SectionTypes[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid
                });
            ExpectedSectionStringArrays = new string[][]
            {
                new string[]{"|      |","|MV    |","|      |","|    SV|","|      |"},
                new string[]{"|      |","|LN    |","|      |","|    CL|","|      |"}
            };
        }

        [Test]
        public void ReplaceTextWithDrivers()
        {
            var sections = TestTrack.Sections;

            var resultStrings1 = Visualisatie.SetSectionString(Visualisatie.GetSingleSectionStringArray(sections.First.Value.SectionType), "Max Verstappen", "Sebastiaan Vettel");
            var resultStrings2 = Visualisatie.SetSectionString(Visualisatie.GetSingleSectionStringArray(sections.First.Value.SectionType), "Lando Norris", "Charles Leclerc");

            Assert.AreEqual(ExpectedSectionStringArrays[0], resultStrings1);
            Assert.AreEqual(ExpectedSectionStringArrays[1], resultStrings2);


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
        [TestCaseSource("Tracks")]
        public void Visualisation_Setup_TestMaxXY(Track track, int eMaxWidth, int eMaxHeight, int eCursorX, int eCursorY)
        {
            Visualisatie.CalcMaxXY(track, out int maxWidth, out int maxHeight, out int cursorX, out int cursorY);
            Assert.AreEqual(eMaxWidth, maxWidth);
            Assert.AreEqual(eMaxHeight, maxHeight);
            Assert.AreEqual(eCursorX, cursorX);
            Assert.AreEqual(eCursorY, cursorY);
        }


    }
}