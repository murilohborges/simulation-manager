using System.Text.Json.Serialization;

namespace SimulationManager.Application.DTOs.Simulation;

public record FastApiResponseDto
{
    [JsonPropertyName("LHV_fuel")]
    public double LhvFuel { get; init; }

    [JsonPropertyName("air_mass_flow")]
    public double AirMassFlow { get; init; }

    [JsonPropertyName("exhaustion_gas_temperature")]
    public double ExhaustionGasTemperature { get; init; }

    [JsonPropertyName("exhaustion_gas_mass_flow")]
    public double ExhaustionGasMassFlow { get; init; }

    [JsonPropertyName("thermal_charge")]
    public double ThermalCharge { get; init; }

    [JsonPropertyName("saturated_water_mass_flow")]
    public double SaturatedWaterMassFlow { get; init; }

    [JsonPropertyName("make_up_water_mass_flow")]
    public double MakeUpWaterMassFlow { get; init; }

    [JsonPropertyName("cooling_water_mass_flow")]
    public double CoolingWaterMassFlow { get; init; }

    [JsonPropertyName("quality_exhaustion_steam_turbine")]
    public double QualityExhaustionSteamTurbine { get; init; }

    [JsonPropertyName("high_steam_mass_flow")]
    public double HighSteamMassFlow { get; init; }

    [JsonPropertyName("medium_steam_mass_flow")]
    public double MediumSteamMassFlow { get; init; }

    [JsonPropertyName("low_steam_mass_flow")]
    public double LowSteamMassFlow { get; init; }

    [JsonPropertyName("pump_variation_pressure")]
    public double PumpVariationPressure { get; init; }

    [JsonPropertyName("net_power_gas_turbine")]
    public double NetPowerGasTurbine { get; init; }

    [JsonPropertyName("gross_power_steam_turbine")]
    public double GrossPowerSteamTurbine { get; init; }

    [JsonPropertyName("net_power_steam_turbine")]
    public double NetPowerSteamTurbine { get; init; }

    [JsonPropertyName("power_consumed_pump")]
    public double PowerConsumedPump { get; init; }

    [JsonPropertyName("gross_power_cycle_combined")]
    public double GrossPowerCycleCombined { get; init; }

    [JsonPropertyName("net_power_cycle_combined")]
    public double NetPowerCycleCombined { get; init; }

    [JsonPropertyName("gross_cycle_combined_efficiency")]
    public double GrossCycleCombinedEfficiency { get; init; }

    [JsonPropertyName("net_cycle_combined_efficiency")]
    public double NetCycleCombinedEfficiency { get; init; }
}
