using System;
using System.Collections.Generic;
using System.Linq;
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
    public class IT3_UserInterface_CookControl
    {
        private IOutput _output;
        private IDoor _door;
        private ILight _light;
        private IDisplay _display;
        private ITimer _timer;
        private IPowerTube _powerTube;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        private ICookController _cookController;
        private IUserInterface _userInterface;


        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _door = new Door();
            _light = new Light(_output);
            _display = new Display(_output);
            _timer = new Timer();
            _powerTube = new PowerTube(_output);

            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();

            _cookController = new CookController(_timer, _display, _powerTube);
            _userInterface = new UserInterface(_powerButton, _timeButton, _startCancelButton, _door, _display, _light, _cookController);
            ((CookController)_cookController).UI = _userInterface;
        }

        [Test]
        public void CookControl_StartCooking_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            
            _output.Received().OutputLine("PowerTube works with 50 %");
        }

        [Test]
        public void UserInterface_CookingDone_OutputIsCorrect()
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            bool expired = false;
            _timer.Expired += (sender, args) => expired = true;

            _cookController.StartCooking(50, 1);

            pause.WaitOne(1500);

            _output.Received().OutputLine("PowerTube turned off");
        }

        [Test]
        public void CookControl_Stop_OnDoorOpen_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);

            _output.Received().OutputLine("PowerTube turned off");
        }


    }
}
