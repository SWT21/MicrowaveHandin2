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

namespace Mircowave.Test.Integration
{
    [TestFixture]
    public class IT1_UserInterface_LightDisplay
    {
        private IUserInterface _userInterface;
        private ICookController _cookController;
        private ILight _light;
        private IDisplay _display;
        private IOutput _output;
        private IDoor _door;

        private IButton _powerButton;
        private IButton _timeButton;
        private IButton _startCancelButton;

        [SetUp]
        public void SetUp()
        {
            _output = Substitute.For<IOutput>();
            _cookController = Substitute.For<ICookController>();

            _powerButton = new Button();
            _timeButton = new Button();
            _startCancelButton = new Button();
            _door = new Door();
            
            _light = new Light(_output);
            _display = new Display(_output);

            _userInterface = new UserInterface(_powerButton,_timeButton,_startCancelButton,_door,_display,_light,_cookController);
        }

        [Test]
        public void Light_TurnOn_DoorOpen_OutputIsCorrect()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);

            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void Light_TurnOff_DoorClose_OutputIsCorrect()
        {
            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnDoorClosed(_door, EventArgs.Empty);

            _output.Received(1).OutputLine("Light is turned off");
        }

        [Test]
        public void Light_TurnOn_StartCancelButtonPressed_OutputIsCorrect()
        {
            
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.Received().OutputLine("Light is turned on");
        }

        [Test]
        public void Light_TurnOff_StartCancelButtonPressed_WhileCooking_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.Received().OutputLine("Light is turned off");
        }

        [Test]
        public void Display_ShowPower_PowerButtonPressed_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.Received(1).OutputLine("Display shows: 50 W");
        }

        [Test]
        public void Display_ShowTime_PowerButtonPressed_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);

            _output.Received(1).OutputLine("Display shows: 01:00");
        }

        [Test]
        public void Display_Cleared_CookingIsDone_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.CookingIsDone();

            _output.Received().OutputLine("Display cleared");

        }

        [Test]
        public void Display_Cleared_DoorOpenWhileCooking_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnDoorOpened(_door, EventArgs.Empty);
            
            _output.Received().OutputLine("Display cleared");

        }

        [Test]
        public void Display_Cleared_StartCancelButtonPressed_WhileCooking_OutputIsCorrect()
        {
            _userInterface.OnPowerPressed(_powerButton, EventArgs.Empty);
            _userInterface.OnTimePressed(_timeButton, EventArgs.Empty);
            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);
            _output.ClearReceivedCalls();

            _userInterface.OnStartCancelPressed(_startCancelButton, EventArgs.Empty);

            _output.Received().OutputLine("Display cleared");

        }
    }
}
