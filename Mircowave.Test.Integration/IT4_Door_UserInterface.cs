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
    public class IT4_Door_UserInterface
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
        public void Door_Open_OnDoorOpened_OutputIsCorrect()
        {
            _door.Open();
            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void Door_Close_OnDoorClosed_OutputIsCorrect()
        {
            _door.Open();
            _door.Close();
            _output.Received().OutputLine("Light is turned off");
        }
    }
}
