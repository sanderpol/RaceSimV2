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
            TestTrack = new Track("StartGrid", 1, new SectionTypes[]
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

            Assert.AreEqual(ExpectedSectionStringArrays[1], resultStrings1);
            Assert.AreEqual(ExpectedSectionStringArrays[2], resultStrings2);


        }
    }
}