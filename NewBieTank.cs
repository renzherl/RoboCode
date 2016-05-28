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
        Enemy enemy = new Enemy();
        double previousEnergy = 100;
        int movementDirection = 1;
        int gunDirection = 1;
        

        public override void Run()
        {
            IsAdjustGunForRobotTurn = true;
            //IsAdjust
            SetTurnGunRight(99999);
            while (true)
            {
                if (enemy.name == null)
                {
                    SetTurnGunRight(2 * Math.PI);
                }
                Execute();


            }
            
        }

        // Robot event handler, when the robot sees another robot
        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            enemy.update(e, this);
            double Offset = rectify(enemy.direction - RadarHeadingRadians);
            //SetTurnRadarRightRadians(Offset * 1.5);
            SetTurnGunRight(Offset * 1.5);
            
            Fire(2);
            //// Stay at right angles to the opponent
            
            SetTurnRight(e.Bearing + 90 - 30 * movementDirection);
            double changeInEnergy = previousEnergy - e.Energy;
            if (changeInEnergy > 0 && changeInEnergy <= 3) 
            {
         
                movementDirection = -movementDirection;
                SetAhead((e.Distance / 4 + 25) * movementDirection);
                enemy.update(e, this);
                Offset = rectify(enemy.direction - RadarHeadingRadians);
                //SetTurnRadarRightRadians(Offset * 1.5);
                SetTurnGunRight(Offset * 1.5);
            }
            gunDirection = -gunDirection;
            //SetTurnGunRight(99999 * gunDirection);          
            previousEnergy = e.Energy;
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

        public double rectify(double angle)
        {
            if (angle < -Math.PI)
                angle += 2 * Math.PI;
            if (angle > Math.PI)
                angle -= 2 * Math.PI;
            return angle;
        }

    }
}
