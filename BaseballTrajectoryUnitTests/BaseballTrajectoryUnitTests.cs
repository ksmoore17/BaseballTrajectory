using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BaseballTrajectory;

namespace BaseballTrajectoryUnitTests
{
    [TestClass]
    public class TrajectoryUnitTests
    {
        [TestMethod]
        public void TestCalculateTrajectory_1()
        {
            // Test with default and no direction hit
            Trajectory trajectory = new Trajectory(100, 30);

            trajectory.Calculate();

            Assert.AreEqual(546, trajectory.Positions.Count);

            Vector positionFinal = trajectory.Positions[5.45];
            Assert.AreEqual(0, positionFinal.X);
            Assert.AreEqual(385.697776, Math.Round(positionFinal.Y, 6));
            Assert.AreEqual(-0.476315, Math.Round(positionFinal.Z, 6));

            Vector positionLanding = trajectory.LandingPosition;
            Assert.AreEqual(0, positionLanding.X);
            Assert.AreEqual(385.300160, Math.Round(positionLanding.Y, 6));
            Assert.AreEqual(0, positionLanding.Z);

            Assert.AreEqual(5.442148, Math.Round(trajectory.HangTime, 6));
        }

        [TestMethod]
        public void TestCalculateTrajectory_2()
        {
            // Test with non zero hit direction
            Trajectory trajectory = new Trajectory(100, 30, 5);

            trajectory.Calculate();

            Assert.AreEqual(546, trajectory.Positions.Count);

            Vector positionFinal = trajectory.Positions[5.45];
            Assert.AreEqual(36.401743, Math.Round(positionFinal.X, 6));
            Assert.AreEqual(384.140629, Math.Round(positionFinal.Y, 6));
            Assert.AreEqual(-0.406434, Math.Round(positionFinal.Z, 6));

            Vector positionLanding = trajectory.LandingPosition;
            Assert.AreEqual(36.373653, Math.Round(positionLanding.X, 6));
            Assert.AreEqual(383.802719, Math.Round(positionLanding.Y, 6));
            Assert.AreEqual(0, positionLanding.Z);

            Assert.AreEqual(5.443298, Math.Round(trajectory.HangTime, 6));
        }
    }
}
