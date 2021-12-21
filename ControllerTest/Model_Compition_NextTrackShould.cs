using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    class Model_Competition_NextTrackShould
    {
        private Competition _competition;
        private Track elburg;
        private Track oostendorp;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
            elburg = new Track("Circuit Elburg", 0,3, new SectionTypes[]
                {
                    SectionTypes.StartGrid, SectionTypes.Finish, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.LeftCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.RightCorner,
                    SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.StartGrid, SectionTypes.StartGrid
                });

            oostendorp = new Track("Oostendorp", 0,2, new SectionTypes[]
                {
                    SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.StartGrid, SectionTypes.Finish,
                    SectionTypes.Straight, SectionTypes.LeftCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight,
                    SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.RightCorner,
                    SectionTypes.RightCorner, SectionTypes.LeftCorner, SectionTypes.LeftCorner,
                    SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight,
                    SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight
                });
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            _competition.Tracks.Enqueue(elburg);
            var result = _competition.NextTrack();
            Assert.AreEqual(elburg, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            _competition.Tracks.Enqueue(elburg);
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            _competition.Tracks.Enqueue(elburg);
            _competition.Tracks.Enqueue(oostendorp);

            var result1 = _competition.NextTrack();
            var result2 = _competition.NextTrack();

            Assert.AreEqual(elburg, result1);
            Assert.AreEqual(oostendorp, result2);
        }
    }
}