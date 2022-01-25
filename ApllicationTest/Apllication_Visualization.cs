using Application;
using Model;
using NUnit.Framework;
using System;

namespace ApllicationTest
{
    public class Apllication_Visualization
    {

        [SetUp]
        public void Setup()
        {

        }
        [Test]
        [TestCase(500, 0, 26, 26, 52, 52)]
        [TestCase(500, 1, 52, 52, 26, 26)]
        [TestCase(500, 2, 52, 52, 26, 26)]
        [TestCase(500, 3, 26, 26, 52, 52)]
        public void Visualisation_GetDriverCoordinateStart(int distance, int dir, int eLeftX, int eLeftY, int eRightX, int eRightY)
        {
            int cX, cY;
            Section straigt = new Section(SectionTypes.StartGrid);
            SectionData straightData = new SectionData()
            {
                DistanceLeft = distance,
                DistanceRight = distance,
                Left = new Driver("L", 0),
                Right = new Driver("R", 0)
            };

            Visualization.CalculatePosition(straigt, straightData, dir, true, true, out cX, out cY);
            Assert.AreEqual(eLeftX, cX);
            Assert.AreEqual(eLeftY, cY);
            Visualization.CalculatePosition(straigt, straightData, dir, false, true, out cX, out cY);
            Assert.AreEqual(eRightX, cX);
            Assert.AreEqual(eRightY, cY);

        }


        [Test]
        [TestCase(500, 0, 26, 54, 52, 54)]
        [TestCase(1000, 0, 26, 27, 52, 27)]
        [TestCase(1500, 0, 26, 0, 52, 0)]
        [TestCase(500, 1, 26, 26, 26, 52)]
        [TestCase(1000, 1, 53, 26, 53, 52)]
        [TestCase(1500, 1, 80, 26, 80, 52)]
        [TestCase(500, 2, 52, 26, 26, 26)]
        [TestCase(1000, 2, 52, 53, 26, 53)]
        [TestCase(1500, 2, 52, 80, 26, 80)]
        [TestCase(500, 3, 54, 52, 54, 26)]
        [TestCase(1000, 3, 27, 52, 27, 26)]
        [TestCase(1500, 3, 0, 52, 0, 26)]
        public void Visualisation_GetDriverCoordinateStraight(int distance, int dir, int eLeftX, int eLeftY, int eRightX, int eRightY)
        {
            int cX, cY;
            Section straigt = new Section(SectionTypes.Straight);
            SectionData straightData = new SectionData()
            {
                DistanceLeft = distance,
                DistanceRight = distance,
                Left = new Driver("L", 0),
                Right = new Driver("R", 0)
            };

            Visualization.CalculatePosition(straigt, straightData, dir, true, false, out cX, out cY);
            Assert.AreEqual(eLeftX, cX);
            Assert.AreEqual(eLeftY, cY);
            Visualization.CalculatePosition(straigt, straightData, dir, false, false, out cX, out cY);
            Assert.AreEqual(eRightX, cX);
            Assert.AreEqual(eRightY, cY);

        }

        [Test]
        [TestCase(0, 0, 25, 80, 51, 80)]
        [TestCase(500, 0, 22, 68, 45, 55)]
        [TestCase(1000, 0, 13, 58, 26, 35)]
        [TestCase(1500, 0, 0, 54, 0, 28)]
        [TestCase(0, 1, 0, 25, 0, 51)]
        [TestCase(500, 1, 13, 22, 26, 45)]
        [TestCase(1000, 1, 22, 12, 45, 25)]
        [TestCase(1500, 1, 26, 0, 52, 0)]
        [TestCase(0, 2, 29, 0, 55, 0)]
        [TestCase(500, 2, 35, 25, 58, 12)]
        [TestCase(1000, 2, 55, 45, 68, 22)]
        [TestCase(1500, 2, 80, 52, 80, 26)]
        [TestCase(0, 3, 80, 29, 80, 55)]
        [TestCase(500, 3, 54, 35, 67, 58)]
        [TestCase(1000, 3, 35, 54, 58, 67)]
        [TestCase(1500, 3, 28, 80, 54, 80)]
        public void Visualisation_GetDriverCoordinateLeftCorner(int distance, int dir, int eLeftX, int eLeftY, int eRightX, int eRightY)
        {
            int cX, cY;
            Section straigt = new Section(SectionTypes.LeftCorner);
            SectionData straightData = new SectionData()
            {
                DistanceLeft = distance,
                DistanceRight = distance,
                Left = new Driver("L", 0),
                Right = new Driver("R", 0)
            };

            Visualization.CalculatePosition(straigt, straightData, dir, true, false, out cX, out cY);
            Assert.AreEqual(eLeftX, cX);
            Assert.AreEqual(eLeftY, cY);
            Visualization.CalculatePosition(straigt, straightData, dir, false, false, out cX, out cY);
            Assert.AreEqual(eRightX, cX);
            Assert.AreEqual(eRightY, cY);
        }

        [Test]
        [TestCase(0, 0, 29, 80, 55, 80)]
        [TestCase(500, 0, 35, 54, 58, 67)]
        [TestCase(1000, 0, 54, 35, 67, 58)]
        [TestCase(1500, 0, 80, 28, 80, 54)]
        [TestCase(0, 1, 0, 29, 0, 55)]
        [TestCase(500, 1, 26, 35, 13, 58)]
        [TestCase(1000, 1, 45, 54, 22, 67)]
        [TestCase(1500, 1, 52, 80, 26, 80)]
        [TestCase(0, 2, 25, 0, 51, 0)]
        [TestCase(500, 2, 22, 12, 45, 25)]
        [TestCase(1000, 2, 13, 22, 26, 45)]
        [TestCase(1500, 2, 0, 26, 0, 52)]
        [TestCase(0, 3, 80, 25, 80, 51)]
        [TestCase(500, 3, 68, 22, 55, 45)]
        [TestCase(1000, 3, 58, 12, 35, 25)]
        [TestCase(1500, 3, 54, 0, 28, 0)]
        public void Visualisation_GetDriverCoordinateRightCorner(int distance, int dir, int eLeftX, int eLeftY, int eRightX, int eRightY)
        {
            int cX, cY;
            Section straigt = new Section(SectionTypes.RightCorner);
            SectionData straightData = new SectionData()
            {
                DistanceLeft = distance,
                DistanceRight = distance,
                Left = new Driver("L", 0),
                Right = new Driver("R", 0)
            };

            Visualization.CalculatePosition(straigt, straightData, dir, true, false, out cX, out cY);
            Assert.AreEqual(eLeftX, cX);
            Assert.AreEqual(eLeftY, cY);
            Visualization.CalculatePosition(straigt, straightData, dir, false, false, out cX, out cY);
            Assert.AreEqual(eRightX, cX);
            Assert.AreEqual(eRightY, cY);

        }





    }
}