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
    public class IT5_PowerTubeCookController
    {
        private IUserInterface _ui;
        private ITimer _timer;
        private IDisplay _display;
        private IPowerTube _powerTube;

        private CookController _cookController;

        [SetUp]
        public void SetUp()
        {
            _ui = Substitute.For<IUserInterface>();
            _timer = Substitute.For<ITimer>();
            _display = Substitute.For<IDisplay>();
            _powerTube = Substitute.For<IPowerTube>();

            _cookController = new CookController(_timer, _display, _powerTube, _ui);
        }

        [TestCase(500, 5)]
        public void PowerTube_StartCooking_TurnOnPowerTube(int power, int time)
        {
            _cookController.StartCooking(power, time);
            _powerTube.Received(1).TurnOn(power);
        }

        [TestCase(500, 5)]
        public void PowerTube_OnTimerExpired_TurnOffPowerTube(int power, int time)
        {
            _cookController.StartCooking(power, time);
            _timer.Expired += Raise.EventWith(this, EventArgs.Empty);
            _powerTube.Received(1).TurnOff();
        }

        [Test]
        public void PowerTube_Stop_TurnOffPowerTube()
        {
            _cookController.Stop();
            _powerTube.Received(1).TurnOff();
        }
    }
}
