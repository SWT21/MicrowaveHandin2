using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Mircowave.Test.Integration
{
    [TestFixture]
    public class IT3_OutputPowerTube
    {
        private IOutput _output;
        private IPowerTube _powerTube;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<Output>();
            _powerTube = new PowerTube(_output);
        }

        [Test]
        public void PowerTube_TurnOn_InRange_OutputIsCorrect()
        {
            _powerTube.TurnOn(45);
            _output.Received(1).OutputLine("PowerTube works with 45 %");
        }

        [Test]
        public void PowerTube_TurnOn_InRangeLowest_OutputIsCorrect()
        {
            _powerTube.TurnOn(1);
            _output.Received(1).OutputLine("PowerTube works with 1 %");
        }

        [Test]
        public void PowerTube_TurnOn_InRangeHighest_OutputIsCorrect()
        {
            _powerTube.TurnOn(100);
            _output.Received(1).OutputLine("PowerTube works with 100 %");
        }

        [Test]
        public void PowerTube_TurnOn_OutOfRangeLow_OutputIsCorrect()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _powerTube.TurnOn(0));
        }

        [Test]
        public void PowerTube_TurnOn_OutOfRangeHigh_OutputIsCorrect()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _powerTube.TurnOn(101));
        }

        [Test]
        public void PowerTube_TurnOn_AlreadyOn_OutputIsCorrect()
        {
            _powerTube.TurnOn(45);
            Assert.Throws<ApplicationException>(() => _powerTube.TurnOn(45));
        }

        [Test]
        public void PowerTube_TurnOff_AlreadyOn_OutputIsCorrect()
        {
            _powerTube.TurnOn(45);
            _powerTube.TurnOff();
            _output.Received(1).OutputLine("PowerTube turned off");
        }

    }

}
