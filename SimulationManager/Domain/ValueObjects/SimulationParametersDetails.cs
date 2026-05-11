namespace SimulationManager.Domain.ValueObjects
{
    public class SimulationParametersDetails
    {
        public double FuelMassFlow { get; set; }
        public double FuelInputTemperature { get; set; }
        public double AirInputTemperature { get; set; }
        public double PercentExcessAir { get; set; }
        public double LocalAtmosphericPressure { get; set; }
        public double LocalTemperature { get; set; }
        public double RelativeHumidity { get; set; }
        public double GasTurbineEfficiency { get; set; }
        public double ChimneyGasTemperature { get; set; }
        public double PurgeLevel { get; set; }
        public double HighSteamLevelPressure { get; set; }
        public double MediumSteamLevelPressure { get; set; }
        public double LowSteamLevelPressure { get; set; }
        public double HighSteamLevelTemperature { get; set; }
        public double MediumSteamLevelTemperature { get; set; }
        public double LowSteamLevelTemperature { get; set; }
        public double HighSteamLevelFraction { get; set; }
        public double MediumSteamLevelFraction { get; set; }
        public double HighSteamLevelEfficiency { get; set; }
        public double MediumSteamLevelEfficiency { get; set; }
        public double LowSteamLevelEfficiency { get; set; }
        public double ReductorGeneratorSetEfficiency { get; set; }
        public double PumpEfficiency { get; set; }
        public double EnginePumpEfficiency { get; set; }
        public double PowerFactorPumpEfficiency { get; set; }
        public double CondenserOperationPressure { get; set; }
        public double RangeTemperatureCoolingWater { get; set; }
    }
}
