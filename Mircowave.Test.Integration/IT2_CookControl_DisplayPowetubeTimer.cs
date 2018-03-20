using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using MicrowaveOvenClasses.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Mircowave.Test.Integration
{
    [TestFixture]
    public class IT2_CookControl_DisplayPowetubeTimer
    {
        private CookController _cookController;
        private IUserInterface _userInterface;
        private IDisplay _display;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IOutput _output;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _userInterface = Substitute.For<IUserInterface>();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _timer = new Timer();

            _cookController = new CookController(_timer, _display, _powerTube, _userInterface);
        }

        [Test]
        public void Display_ShowTime_OnTimerTick_OutputIsCorrect()
        {
            _cookController.StartCooking(50,500);
            _output.ClearReceivedCalls();

            _cookController.OnTimerTick(_timer, EventArgs.Empty);

            _output.Received().OutputLine("Display shows: 08:20");
        }

        [Test]
        public void PowerTube_TurnOn_StartCooking_OutputIsCorrect()
        {
            _cookController.StartCooking(350, 500);

            _output.Received().OutputLine("PowerTube works with 50 %");
        }

        [Test]
        public void PowerTube_TurnOff_TimerExpired_OutputIsCorrect()
        {
            _cookController.StartCooking(50, 500);
            _cookController.OnTimerTick(_timer, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _cookController.OnTimerExpired(_timer, EventArgs.Empty);

            _output.Received().OutputLine("PowerTube turned off");
        }

        [Test]
        public void PowerTube_TurnOff_StoppedWhileCooking_OutputIsCorrect()
        {
            _cookController.StartCooking(50, 500);
            _output.ClearReceivedCalls();
            _cookController.Stop();

            _output.Received().OutputLine("PowerTube turned off");
        }

        [TestCase(2, 1300, 1)]
        [TestCase(5, 3300, 3)]
        public void Timer_TimerTicks_StartCooking_TimerTicksCorrectly(int time, int waitTime, int expectedTicks)
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            int ticks = 0;
            _timer.TimerTick += (sender, args) => ticks++;

            _cookController.StartCooking(50, time);
            pause.WaitOne(waitTime);
            Assert.That(ticks, Is.EqualTo(expectedTicks));
        }

        [Test]
        public void Timer_TimerExpired_StartCooking_ExpiredIsTrue()
        {
            ManualResetEvent pause = new ManualResetEvent(false);
            bool expired = false;
            _timer.Expired += (sender, args) => expired = true;

            _cookController.StartCooking(50, 2);
            pause.WaitOne(2100);
            Assert.That(expired, Is.EqualTo(true));
        }

    }
}
