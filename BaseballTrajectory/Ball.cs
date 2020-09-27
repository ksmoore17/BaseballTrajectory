using System;

namespace BaseballTrajectory
{
    public class Ball
    {
        //Mass in ounces
        public double Mass
        { get; set; }

        //Circumference in inches
        public double Circumference
        { get; set; }

        public Ball(double mass = 5.125, double circumference = 9.125)
        {
            Mass = mass;
            Circumference = circumference;
        }
    }
}
