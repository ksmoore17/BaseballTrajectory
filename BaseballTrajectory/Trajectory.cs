using System;
namespace BaseballTrajectory
{
    public class Trajectory
    {
        //
        private double dragCoefficient = .4105;
        private double dragCoefficientDot = .0044;
        private double dragCoefficientSpin = .2017;

        //s
        private double tau = 25;

        //Constant for coefficients in 1/ft
        private double c0;

        public Ball Ball { get; set; }
        public Environment Environment { get; set; }
        public Impact Impact { get; set; }

        public Trajectory(double exitVelocity, double launchAngle, double direction)
        {
            Impact = new Impact(exitVelocity, launchAngle, direction);
            Ball = new Ball();
            Environment = new Environment();
        }

        public Trajectory(double exitVelocity, double launchAngle)
        {
            Impact = new Impact(exitVelocity, launchAngle, 0);
            Ball = new Ball();
            Environment = new Environment();
        }

        public Trajectory(Impact impact)
        {
            Impact = impact;
            Ball = new Ball();
            Environment = new Environment();
        }

        public Trajectory(Impact impact, Ball ball)
        {
            Impact = impact;
            Ball = ball;
            Environment = new Environment();
        }

        public Trajectory(Impact impact, Environment environment)
        {
            Impact = impact;
            Ball = new Ball();
            Environment = environment;
        }

        public Trajectory(Impact impact, Ball ball, Environment environment)
        {
            Impact = impact;
            Ball = ball;
            Environment = environment;
        }

        public void Calculate()
        {
            c0 = 0.07182 * Environment.Rho * (5.125 / Ball.Mass) * Math.Pow((Ball.Circumference / 9.125), 2);
        }
    }
}
