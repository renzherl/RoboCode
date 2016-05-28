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

        public override void Run()
        {
            
            BodyColor = (Color.Yellow);
            GunColor = (Color.Black);
            RadarColor = (Color.Green);
            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForGunTurn = true;
          
            while (true)
            {
                
                    if (enemy.name == null)
                    {
                        SetTurnRadarRightRadians(2 * Math.PI);
                        Execute();
                    }
                    else
                    {
                        Execute();
                    }
            }
        }

        // Robot event handler, when the robot sees another robot
        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            enemy.update(e, this);
            double Offset = rectify(enemy.direction - RadarHeadingRadians);
            SetTurnRadarRightRadians(Offset * 1.5);
            //smartFire(e.Distance);
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
