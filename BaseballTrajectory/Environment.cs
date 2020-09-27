using System;

namespace BaseballTrajectory
{
    public class Environment
    {
        public double Temperature
        { get; set; }

        public double Altitude
        { get; set; }

        public Environment()
        {
            Temperature = 30;
            Altitude = 100;

        }
    }
}
