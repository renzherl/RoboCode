using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robocode;
using Robocode.Util;


namespace NewbieTank
{
    public class Enemy
    {
        public double x, y;
        public String name = null;
        public double headingRadian = 0.0D;
        public double bearingRadian = 0.0D;
        public double distance = 1000D;
        public double direction = 0.0D;
        public double velocity = 0.0D;
        public double prevHeadingRadian = 0.0D;
        public double energy = 100.0D;

        public void update(ScannedRobotEvent e, AdvancedRobot me)
        {
            name = e.Name;
            headingRadian = e.HeadingRadians;
            bearingRadian = e.BearingRadians;
            this.energy = e.Energy;
            this.velocity = e.Velocity;
            this.distance = e.Distance;
            direction = bearingRadian + me.HeadingRadians;
            x = me.X + Math.Sin(direction) * distance;
            y = me.Y + Math.Cos(direction) * distance;
        }
    }
}
