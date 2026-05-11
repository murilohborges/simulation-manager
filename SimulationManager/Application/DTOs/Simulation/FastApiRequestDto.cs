using System.Text.Json.Serialization;

namespace SimulationManager.Application.DTOs.Simulation;

public record FastApiRequestDto
{
    // FuelComposition's fields
    [JsonPropertyName("methane_molar_fraction_fuel")]
    public double MethaneMolarFraction { get; init; }

    [JsonPropertyName("ethane_molar_fraction_fuel")]
    public double EthaneMolarFraction { get; init; }

    [JsonPropertyName("propane_molar_fraction_fuel")]
    public double PropaneMolarFraction { get; init; }

    [JsonPropertyName("n_butane_molar_fraction_fuel")]
    public double NButaneMolarFraction { get; init; }

    [JsonPropertyName("water_molar_fraction_fuel")]
    public double WaterMolarFraction { get; init; }

    [JsonPropertyName("carbon_dioxide_molar_fraction_fuel")]
    public double CarbonDioxideMolarFraction { get; init; }

    [JsonPropertyName("hydrogen_molar_fraction_fuel")]
    public double HydrogenMolarFraction { get; init; }

    [JsonPropertyName("nitrogen_molar_fraction_fuel")]
    public double NitrogenMolarFraction { get; init; }

    // Parameters's fields
    [JsonPropertyName("fuel_mass_flow")]
    public double FuelMassFlow { get; init; }

    [JsonPropertyName("fuel_input_temperature")]
    public double FuelInputTemperature { get; init; }

    [JsonPropertyName("air_input_temperature")]
    public double AirInputTemperature { get; init; }

    [JsonPropertyName("percent_excess_air")]
    public double PercentExcessAir { get; init; }

    [JsonPropertyName("local_atmospheric_pressure")]
    public double LocalAtmosphericPressure { get; init; }

    [JsonPropertyName("local_temperature")]
    public double LocalTemperature { get; init; }

    [JsonPropertyName("relative_humidity")]
    public double RelativeHumidity { get; init; }

    [JsonPropertyName("gas_turbine_efficiency")]
    public double GasTurbineEfficiency { get; init; }

    [JsonPropertyName("chimney_gas_temperature")]
    public double ChimneyGasTemperature { get; init; }

    [JsonPropertyName("purge_level")]
    public double PurgeLevel { get; init; }

    [JsonPropertyName("high_steam_level_pressure")]
    public double HighSteamLevelPressure { get; init; }

    [JsonPropertyName("medium_steam_level_pressure")]
    public double MediumSteamLevelPressure { get; init; }

    [JsonPropertyName("low_steam_level_pressure")]
    public double LowSteamLevelPressure { get; init; }

    [JsonPropertyName("high_steam_level_temperature")]
    public double HighSteamLevelTemperature { get; init; }

    [JsonPropertyName("medium_steam_level_temperature")]
    public double MediumSteamLevelTemperature { get; init; }

    [JsonPropertyName("low_steam_level_temperature")]
    public double LowSteamLevelTemperature { get; init; }

    [JsonPropertyName("high_steam_level_fraction")]
    public double HighSteamLevelFraction { get; init; }

    [JsonPropertyName("medium_steam_level_fraction")]
    public double MediumSteamLevelFraction { get; init; }

    [JsonPropertyName("high_steam_level_efficiency")]
    public double HighSteamLevelEfficiency { get; init; }

    [JsonPropertyName("medium_steam_level_efficiency")]
    public double MediumSteamLevelEfficiency { get; init; }

    [JsonPropertyName("low_steam_level_efficiency")]
    public double LowSteamLevelEfficiency { get; init; }

    [JsonPropertyName("reductor_generator_set_efficiency")]
    public double ReductorGeneratorSetEfficiency { get; init; }

    [JsonPropertyName("pump_efficiency")]
    public double PumpEfficiency { get; init; }

    [JsonPropertyName("engine_pump_efficiency")]
    public double EnginePumpEfficiency { get; init; }

    [JsonPropertyName("power_factor_pump_efficiency")]
    public double PowerFactorPumpEfficiency { get; init; }

    [JsonPropertyName("condenser_operation_pressure")]
    public double CondenserOperationPressure { get; init; }

    [JsonPropertyName("range_temperature_cooling_tower")]
    public double RangeTemperatureCoolingWater { get; init; }
}
