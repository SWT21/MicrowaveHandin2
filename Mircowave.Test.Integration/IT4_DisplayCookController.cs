using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Mircowave.Test.Integration
{
    [TestFixture]
    public class IT4_DisplayCookController
    {
        private IDisplay _display;
        private ITimer _timer;
        private CookController _cookController;

        [SetUp]
        public void SetUp()
        {
            _display = Substitute.For<IDisplay>();
            _timer = Substitute.For<ITimer>();
            _cookController = new CookController(_timer, _display, new PowerTube(new Output()));
        }

        [TestCase(120, 2, 0)]
        [TestCase(75, 1, 15)]
        [TestCase(60, 1, 0)]
        [TestCase(30, 0, 30)]
        public void Display_OnTimerTick_ShowRemainingTime(int remaningSeconds, int min, int sec)
        {
            _timer.TimeRemaining.Returns(remaningSeconds);
            _cookController.OnTimerTick(_timer, EventArgs.Empty);
            _display.Received(1).ShowTime(min, sec);
        }
    }
}
