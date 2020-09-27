using System;

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

        public double BackSpin
        { get; set; }

        public Impact(double exitVelocity, double launchAngle, double direction)
        {
            ExitVelocity = exitVelocity;
            LaunchAngle = launchAngle;

            BackSpin = 2072 * (ExitVelocity / 100) * (LaunchAngle - 7.2) / (27.5 - 7.2);
        }
    }
}
