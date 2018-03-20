using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrowaveOvenClasses.Boundary;
using MicrowaveOvenClasses.Controllers;
using Timer = MicrowaveOvenClasses.Boundary.Timer;

namespace Microwave.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            ManualResetEvent pause = new ManualResetEvent(false);

            bool extension1 = true;
            bool extension2 = true;
            bool extension3 = true;
            bool extension4 = true;

            var output = new Output();
            var door = new Door();
            var powerButton = new Button();
            var timeButton = new Button();
            var startCancelButton = new Button();
            var light = new Light(output);
            var powerTube = new PowerTube(output);
            var display = new Display(output);
            var timer = new Timer();

            var cookController = new CookController(timer, display, powerTube);
            var userInterface = new UserInterface(powerButton, timeButton, startCancelButton, door, display, light, cookController);
            ((CookController)cookController).UI = userInterface;

            Console.WriteLine("The user opens the door");
            door.Open();
            Console.WriteLine("The user places the dish in the oven");

            UC_STEP4:
            Console.WriteLine("The user closes the door");
            door.Close();

            UC_STEP6:
            Console.WriteLine("Power button is pressed 5 times");
            for (int i = 0; i < 5; i++)
            {
                powerButton.Press();
            }

            if (extension1)
            {
                Console.WriteLine("Start/cancel button is pressed (Extension 1)");
                startCancelButton.Press();
                Console.WriteLine("Continuing from UC STEP 6 ...\n");
                extension1 = false;
                goto UC_STEP6;
            }

            if (extension2)
            {
                Console.WriteLine("The user opens the door during setup (Extension 2)");
                door.Open();
                Console.WriteLine("Continuing from UC STEP 4 ...\n");
                extension2 = false;
                goto UC_STEP4;
            }


            Console.WriteLine("Time button is pressed once");
            timeButton.Press();
            Console.WriteLine("Start/cancel button is pressed");
            startCancelButton.Press();
            
            
            if (extension3)
            {
                pause.WaitOne(10000);
                Console.WriteLine("User suddenly presses start/cancel button (Extension 3)");
                startCancelButton.Press();
                extension3 = false;
                goto UC_STEP6;

            }

            if (extension4)
            {
                pause.WaitOne(10000);
                Console.WriteLine("User suddenly opens the door (Extension 4)");
                door.Open();
                extension4 = false;
                goto UC_STEP4;
            }

            System.Console.ReadLine();
        }
    }
}

