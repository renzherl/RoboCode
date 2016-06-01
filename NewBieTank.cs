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
    public class NewBieTank_Sylvan : AdvancedRobot
    {
        bool hithithit = false;
        EnemyState enemy = new EnemyState();
        //pattern match
        const int MAX_PATTERN_LENGTH = 30;
        Dictionary<string, int[]> matcher = new Dictionary<string, int[]>(40000);
        string enemyHistory;
        //predict
        static double FIRE_POWER = 3;
        private  double FIRE_SPEED = Rules.GetBulletSpeed(FIRE_POWER);
        private List<PointD> predictions = new List<PointD>();
        //move
        double BASE_MOVEMENT = 180;
        double BASE_TURN = Math.PI / 1.5;
        double movement;

        public override void Run()
        {
            IsAdjustGunForRobotTurn = true;
            IsAdjustRadarForGunTurn = true;
            enemyHistory = "";
            movement = Double.PositiveInfinity;
            SetTurnRadarRight(400);
            do
            {
                Scan();
                if (DistanceRemaining == 0)
                {
                    SetAhead(movement = -movement);
                    SetTurnRightRadians(BASE_TURN);
                    hithithit = false;
                }
            } while (true);

        }

        
        public void onHitWall(HitWallEvent e)
        {
            if (Math.Abs(movement) > BASE_MOVEMENT)
            {
                movement = BASE_MOVEMENT;
            }
        }

        public void onRobotDeath(RobotDeathEvent e)
        {
            SetTurnRadarRight(400);
        }

        public void onHitByBullet(HitByBulletEvent e)
        {
            SetTurnRadarRight(400);
        }

        public void onHitRobot(HitRobotEvent e)
        {
            if (hithithit == false)
            {
                double absoluteBearing = e.BearingRadians + HeadingRadians;
                TurnRadarRightRadians(Utils.NormalRelativeAngle(absoluteBearing - RadarHeadingRadians));
                hithithit = true;
            }
        }

        // Robot event handler, when the robot sees another robot
        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            //update
            enemy.update(e, this);
            //fire
            if (GunTurnRemaining == 0 && Energy > 1)
            {
                smartFire();
            }
            //track
            trackHim();
            // memorize.
            if (enemy.thisStep == Convert.ToChar(-1)) ;
            {
                return;
            }
            record(enemy.thisStep);
            enemyHistory = (char)enemy.thisStep + enemyHistory;
            // aim
            predictions.Clear();
            PointD myP = new PointD(X, Y);
            PointD enemyP = project(myP, enemy.absoluteBearing, e.Distance);
            String pattern = enemyHistory;
            for (double d = 0; d < PointD.Distance(myP, enemyP); d += FIRE_SPEED)
            {
                int nextStep = predict(pattern);
                enemy.decode(nextStep);
                enemyP = project(enemyP, enemy.headingRadian, enemy.velocity);
                predictions.Add(enemyP);
                pattern = (char)nextStep + pattern;
            }

            enemy.absoluteBearing = Math.Atan2(enemyP.X - myP.X, enemyP.Y - myP.Y);
            double gunTurn = enemy.absoluteBearing - GunHeadingRadians;
            SetTurnGunRightRadians(Utils.NormalRelativeAngle(gunTurn));
        }

        public void smartFire()
        {
            FIRE_POWER = Math.Min(Math.Min(Energy / 6d, 1000d / enemy.distance), enemy.energy / 3d);
            FIRE_SPEED = Rules.GetBulletSpeed(FIRE_POWER);
            SetFire(FIRE_POWER);
        }
        public void trackHim() 
        {
            double RadarOffset;
            RadarOffset = Utils.NormalRelativeAngle(enemy.absoluteBearing - RadarHeadingRadians);
            SetTurnRadarRightRadians(RadarOffset * 1.2);
        }

        private void record(int thisStep) 
        {
            int maxLength = Math.Min(MAX_PATTERN_LENGTH, enemyHistory.Length);
            for (int i = 0; i <= maxLength; ++i) 
            {
                string pattern = enemyHistory.Substring(0, i);
                int[] frequencies = matcher[pattern];
                if (frequencies == null) 
                {
                // frequency tables need to hold 21 possible dh values times 17 possible v values
                    frequencies = new int[21 * 17];
                    matcher[pattern] = frequencies;
                }
            ++frequencies[thisStep];
            }

        }

        private int predict(string pattern) {
            int [] frequencies = null;
            for (int patternLength = Math.Min(pattern.Length, MAX_PATTERN_LENGTH); frequencies == null; --patternLength) {
                frequencies = matcher[pattern.Substring(0, patternLength)];
            }
            int nextTick = 0;
            for (int i = 1; i < frequencies.Count(); ++i) {
                if (frequencies[nextTick] < frequencies[i]) {
                    nextTick = i;
                }
            }
            return nextTick;
        }

        private static PointD project(PointD p, double angle,double distance) 
        {
            double x = p.X + distance * Math.Sin(angle);
            double y = p.Y + distance * Math.Cos(angle);
            return new PointD(x, y);
        }
    

    }
}
