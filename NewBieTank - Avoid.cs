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
    public class NewBieTankAvoid: AdvancedRobot
    {
        Enemy enemy = new Enemy();
        double previousEnergy = 100;
        int movementDirection = 1;
        int gunDirection = 1;


        public override void Run()
        {
            //IsAdjustGunForRobotTurn = true;
            //IsAdjustRadarForGunTurn = true;
            SetTurnGunRight(99999);
            //while (true)
            //{
            //    if (enemy.name == null)
            //    {
            //        SetTurnRadarRightRadians(2 * Math.PI);
            //        Execute();
            //    }
            //    else

            //        Execute();
            //}

        }

        // Robot event handler, when the robot sees another robot
        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            //enemy.update(e, this);
            //double Offset = rectify(enemy.direction - RadarHeadingRadians);
            //SetTurnRadarRightRadians(Offset * 1.5);
            ////SetTurnGunRightRadians(Offset * 1.5 + Math.PI);
            //WaitFor(new GunTurnCompleteCondition(this));           
            
            //// Stay at right angles to the opponent

            SetTurnRight(e.Bearing + 90 - 30 * movementDirection);
            double changeInEnergy = previousEnergy - e.Energy;
            if (changeInEnergy > 0 && changeInEnergy <= 3)
            {

                movementDirection = -movementDirection;
                SetAhead((e.Distance / 4 + 25) * movementDirection);
            }
            gunDirection = -gunDirection;
            SetTurnGunRight(99999 * gunDirection);
            Fire(2);
            previousEnergy = e.Energy;
        }

        public override void OnScannedMedicalKit(ScannedMedicalKitEvent e)
        {
            //double absoluteBearing = e.BearingRadians + HeadingRadians;
            //TurnRightRadians(Utils.NormalRelativeAngle(absoluteBearing - RadarHeadingRadians));
            //SetAhead(e.Distance);
            TurnRight(e.Bearing);
            Ahead(e.Distance); 
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
