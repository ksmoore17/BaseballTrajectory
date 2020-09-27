using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BaseballTrajectory;

namespace BaseballTrajectoryUnitTests
{
    [TestClass]
    public class TrajectoryUnitTests
    {
        [TestMethod]
        public void TestCalculateTrajectory()
        {
            Trajectory trajectory = new Trajectory(100, 30);

            trajectory.Calculate();

            Assert.AreEqual(546, trajectory.Positions.Count);

            Vector positionFinal = trajectory.Positions[5.45];
            Assert.AreEqual(0, positionFinal.X);
            Assert.AreEqual(385.697776, Math.Round(positionFinal.Y, 6));
            Assert.AreEqual(-0.476315, Math.Round(positionFinal.Z, 6));
        }
    }
}
