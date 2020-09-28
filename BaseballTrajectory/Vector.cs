using System;
namespace BaseballTrajectory
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }

        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector operator +(Vector v1, Vector v2)
        {
            return new Vector(
               v1.X + v2.X,
               v1.Y + v2.Y,
               v1.Z + v2.Z);
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            return new Vector(
               v1.X - v2.X,
               v1.Y - v2.Y,
               v1.Z - v2.Z);
        }

        public static Vector operator -(Vector v1)
        {
            return new Vector(
               -v1.X,
               -v1.Y,
               -v1.Z);
        }

        public static Vector operator *(Vector v1, double scalar)
        {
            return
               new Vector
               (
                  v1.X * scalar,
                  v1.Y * scalar,
                  v1.Z * scalar
               );
        }

        public static Vector operator *(double scalar, Vector v1)
        {
            return
               new Vector
               (
                  v1.X * scalar,
                  v1.Y * scalar,
                  v1.Z * scalar
               );
        }

        public static Vector operator /(Vector v1, double scalar)
        {
            return
               new Vector
               (
                  v1.X / scalar,
                  v1.Y / scalar,
                  v1.Z / scalar
               );
        }

        public static Vector operator /(double scalar, Vector v1)
        {
            return
               new Vector
               (
                  scalar / v1.X,
                  scalar / v1.Y,
                  scalar / v1.Z
               );
        }

        public double Length()
        {
            return Math.Sqrt(X * X + Y * Y + Z * Z);
        }
    }
}
