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
        double previousEnergy = 100;
        int movementDirection = 1;
        int gunDirection = 1;
        int direction = 1;

        private  const double DOUBLE_PI = (Math.PI * 2);
        private  const double HALF_PI = (Math.PI / 2);
        private  const double WALL_AVOID_INTERVAL = 10;
        private  const double WALL_AVOID_FACTORS = 20;
        private  const double WALL_AVOID_DISTANCE = (WALL_AVOID_INTERVAL * WALL_AVOID_FACTORS);
        public override void Run()
        {

            while (true)
            {
                setTurnRightRadiansOptimal(adjustHeadingForWalls(0));
                SetAhead(100);
                Execute();
            }
            
        }

        

        // Robot event handler, when the robot sees another robot
        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            Fire(2);
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
            previousEnergy = e.Energy;
        }

        public double calculateBearingToXYRadians(double sourceX, double sourceY,
            double sourceHeading, double targetX, double targetY) {
                return normalizeRelativeAngleRadians(
                   Math.Atan2((targetX - sourceX), (targetY - sourceY)) -
                       sourceHeading);
            }
        public double normalizeAbsoluteAngleRadians(double angle) {
           if (angle < 0) {
                return (DOUBLE_PI + (angle % DOUBLE_PI));
            } else {
                return (angle % DOUBLE_PI);
            }
        }
        public static double normalizeRelativeAngleRadians(double angle) {
            double trimmedAngle = (angle % DOUBLE_PI);
            if (trimmedAngle > Math.PI) {
                return -(Math.PI - (trimmedAngle % Math.PI));
            } else if (trimmedAngle < -Math.PI) {
                return (Math.PI + (trimmedAngle % Math.PI));
            } else {
                return trimmedAngle;
            }
        }
        
    private double adjustHeadingForWalls(double heading) {
        double fieldHeight = BattleFieldHeight;
        double fieldWidth = BattleFieldWidth;
        double centerX = (fieldWidth / 2);
        double centerY = (fieldHeight / 2);
        double currentHeading = getRelativeHeadingRadians();
        double x = X;
        double y = Y;
        bool nearWall = false;
        double desiredX;
        double desiredY;
        // If we are too close to a wall, calculate a course toward
        // the center of the battlefield.
        if ((y < WALL_AVOID_DISTANCE)
                || ((fieldHeight - y) < WALL_AVOID_DISTANCE)) {
            desiredY = centerY;
            nearWall = true;
        } else {
            desiredY = y;
        }
        if ((x < WALL_AVOID_DISTANCE)
                || ((fieldWidth - x) < WALL_AVOID_DISTANCE)) {
            desiredX = centerX;
            nearWall = true;
        } else {
            desiredX = x;
        }
        // Determine the safe heading and factor it in with the desired
        // heading if the bot is near a wall
        if (nearWall) {
            double desiredBearing = calculateBearingToXYRadians(x, y,
                    currentHeading, desiredX, desiredY);
            double distanceToWall = Math.Min(Math.Min(x, (fieldWidth - x)),
                    Math.Min(y, (fieldHeight - y)));
            int wallFactor = (int) Math.Min(
                    (distanceToWall / WALL_AVOID_INTERVAL), WALL_AVOID_FACTORS);
            return ((((WALL_AVOID_FACTORS - wallFactor) * desiredBearing) + (wallFactor * heading)) / WALL_AVOID_FACTORS);
        } else {
            return heading;
        }
    }

    public double getRelativeHeadingRadians() {
        double relativeHeading = HeadingRadians;
        if (direction < 1) {
            relativeHeading = normalizeAbsoluteAngleRadians(relativeHeading
                    + Math.PI);
        }
        return relativeHeading;
    }

    public void reverseDirection() {
        double distance = (DistanceRemaining * direction);
        direction *= -1;
        setAhead(distance);
    }

         public void setAhead(double distance) 
    {
        double relativeDistance = (distance * direction);
        SetAhead(relativeDistance);
        if (distance < 0) {
            direction *= -1;
        }
    }

        public void setBack(double distance) {
        double relativeDistance = (distance * direction);
        SetBack(relativeDistance);
        if (distance > 0) {
            direction *= -1;
        }
    }

        public void setTurnLeftRadiansOptimal(double angle) {
        double turn = normalizeRelativeAngleRadians(angle);
        if (Math.Abs(turn) > HALF_PI) {
            reverseDirection();
            if (turn < 0) {
                turn = (HALF_PI + (turn % HALF_PI));
            } else if (turn > 0) {
                turn = -(HALF_PI - (turn % HALF_PI));
            }
        }
        SetTurnLeftRadians(turn);
    }

         public void setTurnRightRadiansOptimal(double angle) {
        double turn = normalizeRelativeAngleRadians(angle);
        if (Math.Abs(turn) > HALF_PI) {
            reverseDirection();
            if (turn < 0) {
                turn = (HALF_PI + (turn % HALF_PI));
            } else if (turn > 0) {
                turn = -(HALF_PI - (turn % HALF_PI));
            }
        }
        SetTurnRightRadians(turn);
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
