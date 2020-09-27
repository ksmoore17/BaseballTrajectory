using System;
using System.Numerics;

namespace BaseballTrajectory
{
    public class Impact
    {
        public double ExitVelocity
        { get; set; }

        public double LaunchAngle
        { get; set; }

        public double Direction
        { get; set; }

        public double SpinBack
        { get; set; }

        public Vector PositionInitial
        { get; set; }

        public Impact(double exitVelocity, double launchAngle, double direction, double x = 0, double y = 2, double z = 3)
        {
            ExitVelocity = exitVelocity;
            LaunchAngle = launchAngle;
            Direction = direction;
            PositionInitial = new Vector(x, y, z);

            SpinBack = 2072 * (ExitVelocity / 100) * (LaunchAngle - 7.2) / (27.5 - 7.2);
        }

        public Impact(double exitVelocity, double launchAngle, double direction, Vector positionInitial)
        {
            ExitVelocity = exitVelocity;
            LaunchAngle = launchAngle;
            Direction = direction;
            PositionInitial = positionInitial;

            SpinBack = 2072 * (ExitVelocity / 100) * (LaunchAngle - 7.2) / (27.5 - 7.2);
        }
    }
}
