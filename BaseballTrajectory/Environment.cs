using System;

namespace BaseballTrajectory
{
    public class Environment
    {
        private const double beta = 0.0001217;

        //Saturation Vapor Pressure in mm*hg
        private readonly double svp;
             
        //Temperature in f
        public double Temperature
        { get; set; }

        //Elevation above sealevel in ft
        public double Elevation
        { get; set; }

        //Barometric pressure in in*Hg
        public double Pressure
        { get; set; }

        //Relative humidity as a %
        public double RelativeHumidity
        { get; set; }

        //Wind velocity in mph
        public double WindVelocity
        { get; set; }

        //Wind direction in degrees from the y axis
        //(0 is to CF, 90 is to 1st base, 180/-180 is to home)
        public double WindDirection
        { get; set; }

        //Height above which wind has an effect in ft
        public double WindHeight
        { get; set; }

        //Air density in lb/ft^3
        public double Rho
        { get; set; }

        public Environment(
            double temperature = 78,
            double elevation = 0,
            double pressure = 29.92,
            double relativeHumidity = 50,
            double windVelocity = 0,
            double windDirection = 0,
            double windHeight = 0
        )
        {
            Temperature = temperature;
            Elevation = elevation;
            Pressure = pressure;
            WindVelocity = windVelocity;
            WindDirection = windDirection;
            WindHeight = windHeight;
            RelativeHumidity = relativeHumidity;

            double temperatureC = (5.0 / 9.0) * (Temperature - 32);
            double pressureInHg = Pressure * 1000 / 39.37;

            svp = 4.5841 * Math.Exp((18.687 - temperatureC / 234.5) * temperatureC / (257.14 + temperatureC));
            Rho = 0.06261 * 1.2929 * (273 / (temperatureC + 273) * (pressureInHg * Math.Exp(-beta * Elevation) - 0.3783 * RelativeHumidity * svp / 100) / 760);
        }
    }
}
