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
    internal class Controller_Data_Tests
    {
        
        [Test]
        public void Data_TestInitialize()
        {
            Data.Initialize(new Competition());
            Assert.IsNotEmpty(Data.Competition.Participants);
            Assert.IsNotEmpty(Data.Competition.Tracks);

        }



    }
}
