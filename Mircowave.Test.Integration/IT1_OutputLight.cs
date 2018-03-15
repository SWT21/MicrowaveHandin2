using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Mircowave.Test.Integration
{
    [TestFixture]
    public class IT1_OutputLight
    {
        private ILight _light;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<Output>();
            _light = new Light(_output);
        }

        [Test]
        public void Light_On_OutputIsCorrect()
        {
            _light.TurnOn();
            _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void Light_On_AlreadyOn_OutputIsCorrect()
        {
            _light.TurnOn();
            _light.TurnOn();
            _output.Received(1).OutputLine("Light is turned on");
        }

        [Test]
        public void Light_Off_OutputIsCorrect()
        {
            _light.TurnOff();
            _output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void Light_Off_AlreadyOff_OutputIsCorrect()
        {
            _light.TurnOff();
            _light.TurnOff();
            _output.Received(1).OutputLine("Light is turned off");
        }
    }
}
