using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;
using Robocode.Util;

namespace NewbieTank
{
    class EnemyState
    {

        public double headingRadian = 0.0D;
        public double bearingRadian = 0.0D;
        public double distance = 0.0D;
        public double absoluteBearing = 0.0D;
        public double x = 0.0D;
        public double y = 0.0D;
        public double velocity = 0.0D;
        public double energy = 100.0D;
        //addition
        public double lastEnemyHeading = 0;
        public int thisStep = 0;

        //the currently data is important, we should get it when we use it.
        public void update(ScannedRobotEvent e, AdvancedRobot me)
        {
            headingRadian = e.HeadingRadians;
            bearingRadian = e.BearingRadians;
            distance = e.Distance;
            absoluteBearing = bearingRadian + me.HeadingRadians;
            x = me.X + Math.Sin(absoluteBearing) * distance;
            y = me.Y + Math.Cos(absoluteBearing) * distance;
            velocity = e.Velocity;
            energy = e.Energy;
            //addition
            thisStep = encode(headingRadian - lastEnemyHeading, velocity);
            lastEnemyHeading = headingRadian;
        }

        public static int encode(double dh, double v)
        {
            if (Math.Abs(dh) > Rules.MAX_TURN_RATE_RADIANS)
            {
                return Convert.ToChar(-1);
            }
            //-10<toDegrees(dh)<10 ; -8<v<8 ;
            //so we add with 10 and 8
            int dhCode = (int)Math.Round(RadianToDegree(dh)) + 10;
            int vCode = (int)Math.Round(v + 8);
            return (char)(17 * dhCode + vCode);
        }

        public void decode(int symbol)
        {
            headingRadian += ConvertToRadians(symbol / 17 - 10);
            velocity = symbol % 17 - 8;
        }

        private static double ConvertToRadians(double angle)
        {
            return (Math.PI / 180) * angle;
        }
        private static double RadianToDegree(double angle)
        {
            return angle * (180.0 / Math.PI);
        }
    }
}
