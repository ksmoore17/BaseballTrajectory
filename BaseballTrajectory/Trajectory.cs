using System;
using System.Collections.Generic;

namespace BaseballTrajectory
{
    public class Trajectory
    {
        private readonly Ball _ball;
        private readonly Environment _environment;
        private readonly Impact _impact;

        // Time since impact for the current step
        private double _timeCurrent = 0;

        // Fitted coefficients (from Nathan)

        private double _dragCoefficientStatic = .4105;
        public double DragCoefficientStatic {
            get {
                return _dragCoefficientStatic;
            } 
            set {
                _dragCoefficientStatic = value;
            }
        }

        private double _dragCoefficientDot = .0044;
        public double DragCoefficientDot {
            get {
                return _dragCoefficientDot;
            } 
            set {
                _dragCoefficientDot = value;
            }
        }

        private double _dragCoefficientSpin = .2017;
        public double DragCoefficientSpin {
            get {
                return _dragCoefficientSpin;
            } 
            set {
                _dragCoefficientSpin = value;
            }
        }
        private double _dragCoefficient;

        // s
        private const double _tau = 25;

        // Constant for coefficient in 1/ft
        private double _c0;

        // Initial position vector
        // Filled by CalculateConstants using Impact input
        private Vector _positionInitial = new Vector();

        // Initial velocity vector
        private Vector _velocityInitial = new Vector();

        // Initial spin vector in cartesian axes
        private Vector _spinCartesian = new Vector();

        // Spin magnitude in rad/s
        private double _omega;

        // Spin magnitude in ft/s
        private double _omegaR;

        // Wind velocity in x and y in ft/s
        private Vector _windVelocity = new Vector();

        // Resulting dictionary of position vectors indexed by time
        private Dictionary<double, Vector> _positions = new Dictionary<double, Vector>();

        // Properties
        public Dictionary<double, Vector> Positions
        {
            get
            {
                return _positions;
            }
        }

        private Vector _landingPosition = new Vector();

        public Vector LandingPosition
        {
            get
            {
                return _landingPosition;
            }
        }

        private double _hangTime;

        public double HangTime
        {
            get
            {
                return _hangTime;
            }
        }

        // Constructors to have many options for input

        public Trajectory(double exitVelocity, double launchAngle, double direction)
        {
            // Just the impact values

            _impact = new Impact(exitVelocity, launchAngle, direction);
            _ball = new Ball();
            _environment = new Environment();
        }

        public Trajectory(double exitVelocity, double launchAngle)
        {
            // Just an exit velocity and launch angle, assuming that the direction is not relevant

            _impact = new Impact(exitVelocity, launchAngle, 0);
            _ball = new Ball();
            _environment = new Environment();
        }

        public Trajectory(Impact impact)
        {
            // Providing an Impact object and assuming default Ball and Environment

            _impact = impact;
            _ball = new Ball();
            _environment = new Environment();
        }

        public Trajectory(Impact impact, Ball ball)
        {
            // Provide an impact and ball and assume a default environment
            _impact = impact;
            _ball = ball;
            _environment = new Environment();
        }

        public Trajectory(Impact impact, Environment environment)
        {
            // Provide an impact and environment and assume a default ball
            _impact = impact;
            _ball = new Ball();
            _environment = environment;
        }

        public Trajectory(Impact impact, Ball ball, Environment environment)
        {
            // Provide and impact, a ball, and an environment
            _impact = impact;
            _ball = ball;
            _environment = environment;
        }

        public void Calculate(double timestep = .01)
        {
            // Calculate the trajectory of a ball

            _timeCurrent = 0;

            CalculateConstants();

            Vector positionCurrent = _positionInitial;
            Vector velocityCurrent = _velocityInitial;

            _positions[0] = positionCurrent;

            Vector accelerationCurrent = CalculateAcceleration(positionCurrent, velocityCurrent);

            int index = 0;
            while (positionCurrent.Z >= 0)
            {
                index += 1;
                _timeCurrent = timestep * index;

                // Update the position and velocity, then the acceleration with this new position and velocity
                positionCurrent = positionCurrent + velocityCurrent * timestep + accelerationCurrent * (timestep * timestep) / 2;
                velocityCurrent = velocityCurrent + accelerationCurrent * timestep;
                accelerationCurrent = CalculateAcceleration(positionCurrent, velocityCurrent);

                // Add this simulated step to the positions dictionary
                _positions[_timeCurrent] = positionCurrent;
            }

            // Calculating the interpolated landing point
            double lastPositiveTime = _timeCurrent - timestep;
            Vector lastPositivePosition = _positions[lastPositiveTime];

            double lastTime = _timeCurrent;
            Vector lastPosition = positionCurrent;

            double interpolationRatio = lastPosition.Z / (lastPosition.Z - lastPositivePosition.Z);

            _landingPosition = lastPosition - interpolationRatio * (lastPosition - lastPositivePosition);

            _hangTime = lastTime - interpolationRatio * (lastTime - lastPositiveTime);
        }

        private void CalculateConstants()
        {
            // Calculate the constants and values that do not change over time

            // A coefficient used for drag and magnus acceleration related to the properties of the air around the ball
            _c0 = 0.07182 * _environment.Rho * (5.125 / _ball.Mass) * Math.Pow((_ball.Circumference / 9.125), 2);

            // This is a drag coefficient that changes relative to exit velocity around 100
            _dragCoefficient = _dragCoefficientStatic * (1 + _dragCoefficientDot * (100 - _impact.ExitVelocity));

            // Vector of initial position
            _positionInitial = _impact.PositionInitial;

            // Direction vector of the inital velocity
            _velocityInitial.X = Math.Cos(_impact.LaunchAngle * Math.PI / 180) * Math.Sin(_impact.Direction * Math.PI / 180);
            _velocityInitial.Y = Math.Cos(_impact.LaunchAngle * Math.PI / 180) * Math.Cos(_impact.Direction * Math.PI / 180);
            _velocityInitial.Z = Math.Sin(_impact.LaunchAngle * Math.PI / 180);

            // Scaling to a madnitude
            _velocityInitial *= 1.467 * _impact.ExitVelocity;

            double spinSide = 0;
            double spinGyro = 0;

            // Cartesian vector of spins
            _spinCartesian.X = (_impact.SpinBack * Math.Cos(_impact.Direction * Math.PI / 180) - spinSide * Math.Sin(_impact.LaunchAngle * Math.PI / 180) * Math.Sin(_impact.Direction * Math.PI / 180) + spinGyro * _velocityInitial.X / _velocityInitial.Length()) * Math.PI / 30;
            _spinCartesian.Y = (-_impact.SpinBack * Math.Sin(_impact.Direction * Math.PI / 180) - spinSide * Math.Sin(_impact.LaunchAngle * Math.PI / 180) * Math.Cos(_impact.Direction * Math.PI / 180) + spinGyro * _velocityInitial.Y / _velocityInitial.Length()) * Math.PI / 30;
            _spinCartesian.Z = (spinSide * Math.Cos(_impact.LaunchAngle * Math.PI / 180) + spinGyro * _velocityInitial.Z / _velocityInitial.Length()) * Math.PI / 30;

            // Spin magnitude in rad/s
            _omega = _spinCartesian.Length();
            // ft/s
            _omegaR = (_ball.Circumference / 2 / Math.PI) * _omega / 12;

            // Wind velocity in ft/s
            _windVelocity.X = _environment.WindVelocity * 1.467 * Math.Sin(_environment.WindDirection * Math.PI / 180);
            _windVelocity.Y = _environment.WindVelocity * 1.467 * Math.Cos(_environment.WindDirection * Math.PI / 180);
        }

        private double CalculateRelativeWindVelocity(Vector velocity, Vector windVelocity, double currentZ)
        {
            // Calculate the velocity of the wind "felt" by the ball

            if (currentZ >= _environment.WindHeight)
            {
                return new Vector(velocity.X - windVelocity.X, velocity.Y - windVelocity.Y, velocity.Z).Length();
            }

            return velocity.Length();
        }
        private Vector CalculateDragAcceleration(double s, double windVelocityRelative, Vector velocity)
        {
            // Calculate the contribution to acceleration from drag which changes wrt velocity
            double dragCoefficient = _dragCoefficient * (1 + _dragCoefficientSpin * s * s);

            // Max function is used to only consider wind if above wind height
            Vector dragAcceleration = new Vector
            {
                X = -_c0 * dragCoefficient * windVelocityRelative * (velocity.X - Math.Max(_windVelocity.X - _environment.WindHeight, 0)),
                Y = -_c0 * dragCoefficient * windVelocityRelative * (velocity.Y - Math.Max(_windVelocity.Y - _environment.WindHeight, 0)),
                Z = -_c0 * dragCoefficient * windVelocityRelative * (velocity.Z)
            };

            return dragAcceleration;
        }

        private Vector CalculateMagnusAcceleration(double s, double windVelocityRelative, Vector velocity)
        {
            // Calculate the contribution to acceleration from magnus effects which changes wrt velocity (lift)

            double liftCoefficient = 1 / (2.32 + 0.4 / s);

            Vector magnusAcceleration = new Vector
            {
                X = 0,
                Y = _c0 * (liftCoefficient / _omega) * windVelocityRelative * (_spinCartesian.Z * (velocity.X - 0) - _spinCartesian.X * velocity.Z),
                Z = _c0 * (liftCoefficient / _omega) * windVelocityRelative * (_spinCartesian.X * (velocity.Y - Math.Max(_windVelocity.Y - _environment.WindHeight, 0)) - _spinCartesian.Y * (velocity.X - 0))
            };

            return magnusAcceleration;
        }

        private Vector CalculateAcceleration(Vector position, Vector velocity)
        {
            // Calculates drag + magnus acceleration, then subtracts for gravity to get the total acceleration
            double windVelocityRelative = CalculateRelativeWindVelocity(velocity, _windVelocity, position.Z);

            double s = (_omegaR / windVelocityRelative) * Math.Exp(-_timeCurrent / (_tau * 146.7 / velocity.Length()));

            Vector dragAcceleration = CalculateDragAcceleration(s, windVelocityRelative, velocity);
            Vector magnusAcceleration = CalculateMagnusAcceleration(s, windVelocityRelative, velocity);

            Vector acceleration = dragAcceleration + magnusAcceleration;
            // Acceleration due to gravity in ft/s^2
            acceleration.Z -= 32.17404855643;

            return acceleration;
        }

    }
}
