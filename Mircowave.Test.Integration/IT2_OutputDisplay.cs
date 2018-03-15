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
    public class IT2_OutputDisplay
    {
        private IDisplay _display;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<Output>();
            _display = new Display(_output);
        }

        [Test]
        public void Display_ShowTime_Zero_OutputIsCorrect()
        {
            _display.ShowTime(0,0);
            _output.Received(1).OutputLine("Display shows: 00:00");
        }

        [Test]
        public void Display_ShowTime_Minutes_OutputIsCorrect()
        {
            _display.ShowTime(7, 0);
            _output.Received(1).OutputLine("Display shows: 07:00");
        }

        [Test]
        public void Display_ShowTime_Seconds_OutputIsCorrect()
        {
            _display.ShowTime(0, 7);
            _output.Received(1).OutputLine("Display shows: 00:07");
        }

        [Test]
        public void Display_ShowTime_MinutesSeconds_OutputIsCorrect()
        {
            _display.ShowTime(12, 45);
            _output.Received(1).OutputLine("Display shows: 12:45");
        }

        [Test]
        public void Display_ShowPower_Zero_OutputIsCorrect()
        {
            _display.ShowPower(0);
            _output.Received(1).OutputLine("Display shows: 0 W");
        }

        [Test]
        public void Display_ShowPower_Fifty_OutputIsCorrect()
        {
            _display.ShowPower(50);
            _output.Received(1).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void Display_Clear_OutputIsCorrect()
        {
            _display.Clear();
            _output.Received(1).OutputLine("Display cleared");
        }
    }
}
