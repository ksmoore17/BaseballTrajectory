using System;

namespace BaseballTrajectory
{
    public class Environment
    {
        // Beta constant
        public double Beta
        { get; set; }

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

        // Convert F to C
        private double TemperatureC
        {
            get
            {
                return (5.0 / 9.0) * (Temperature - 32);
            }
        }

        // Convert in Hg to mm Hg
        private double PressureMmHg
        {
            get
            {
                return Pressure * 1000 / 39.37;
            }
        }

        //Saturation Vapor Pressure in mm*hg
        public double Svp
        {
            get
            {
                double temperatureC = TemperatureC;

                return 4.5841 * Math.Exp((18.687 - temperatureC / 234.5) * temperatureC / (257.14 + temperatureC));
            }
        }

        //Air density in lb/ft^3
        public double Rho
        {
            get
            {
                return 0.06261 * 1.2929 * (273 / (TemperatureC + 273) * (PressureMmHg * Math.Exp(-Beta * Elevation) - 0.3783 * RelativeHumidity * Svp / 100) / 760);
            }
        }

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

            Beta = 0.0001217;
        }
    }
}
