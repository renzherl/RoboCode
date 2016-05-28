using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Robocode;
using Robocode.Util;


namespace NewbieTank
{
    public class NewBieTank : AdvancedRobot
    {
        private bool movingForward;

        // The main method of your robot containing robot logics
        public override void Run()
        {
            // -- Initialization of the robot --
            BodyColor = (Color.Yellow);
            GunColor = (Color.Red);
            // Here we turn the robot to point upwards, and move the gun 90 degrees
            TurnLeft(Heading - 90);
            TurnGunRight(90);

            // Infinite loop making sure this robot runs till the end of the battle round
            while (true)
            {
                // Tell the game we will want to move ahead 40000 -- some large number
                SetAhead(40000);
                movingForward = true;
                // Tell the game we will want to turn right 90
                SetTurnRight(90);
                // At this point, we have indicated to the game that *when we do something*,
                // we will want to move ahead and turn right.  That's what "set" means.
                // It is important to realize we have not done anything yet!
                // In order to actually move, we'll want to call a method that
                // takes real time, such as WaitFor.
                // WaitFor actually starts the action -- we start moving and turning.
                // It will not return until we have finished turning.
                WaitFor(new TurnCompleteCondition(this));
                // Note:  We are still moving ahead now, but the turn is complete.
                // Now we'll turn the other way...
                SetTurnLeft(180);
                // ... and wait for the turn to finish ...
                WaitFor(new TurnCompleteCondition(this));
                // ... then the other way ...
                SetTurnRight(180);
                // .. and wait for that turn to finish.
                WaitFor(new TurnCompleteCondition(this));
                // then back to the top to do it all again
            }
        }

        // Robot event handler, when the robot sees another robot
        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            // We fire the gun with bullet power = 1
            smartFire(e.Distance);
        }

        public void smartFire(double robotDistance)
        {
            if (robotDistance > 200 || Energy < 15)
            {
                Fire(1);
            }
            else if (robotDistance > 50)
            {
                Fire(2);
            }
            else
            {
                Fire(3);
            }
        }

    }
}
