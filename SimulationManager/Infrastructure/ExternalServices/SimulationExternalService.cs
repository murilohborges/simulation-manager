using SimulationManager.Application.DTOs.Simulation;
using SimulationManager.Domain.Exceptions;
using SimulationManager.Domain.Interfaces;
using SimulationManager.Domain.ValueObjects;
using System.Text.Json;

namespace SimulationManager.Infrastructure.ExternalServices;

public class SimulationExternalService : ISimulationExternalService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SimulationExternalService> _logger;

    public SimulationExternalService(
        HttpClient httpClient,
        ILogger<SimulationExternalService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<FastApiResponseDto> ExecuteAsync(
        FuelCompositionDetails composition,
        SimulationParametersDetails parameters)
    {
        try
        {
            // Creating request
            var request = BuildRequest(composition, parameters);

            // Getting response
            var response = await _httpClient.PostAsJsonAsync("/simulation", request);

            // Handle error responses
            if (!response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                _logger.LogError(
                    "FastAPI returned {StatusCode}. Body: {Body}",
                    response.StatusCode,
                    responseBody);

                // Try to parse as FastAPI error structure
                try
                {
                    var errorResponse = JsonSerializer.Deserialize<FastApiErrorResponseDto>(
                        responseBody,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                    );

                    if (errorResponse != null)
                    {
                        List<ValidationErrorDetail> validationErrors = new();

                        // PATTERN 1: Error with details(specific field)
                        if (errorResponse.Details != null && errorResponse.Details.Count > 0)
                        {
                            validationErrors = errorResponse.Details
                                .Select(d => new ValidationErrorDetail(
                                    Field: string.Join(".", d.Loc.Skip(1)),
                                    Message: d.Msg,
                                    Type: d.Type
                                ))
                                .ToList();
                        }

                        // PATTERN 2: Error without details (business logic)
                        else
                        {
                            validationErrors.Add(new ValidationErrorDetail(
                                Field: "general",
                                Message: errorResponse.Error,
                                Type: errorResponse.Type
                            ));
                        }

                        throw new SimulationExecutionException(
                            errorResponse.Error,
                            errorResponse.Type,
                            validationErrors
                        );
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed to parse FastAPI error response as structured error");
                    // If not structured error, fall through to generic error
                }

                // Generic error if can't parse as FastAPI error
                throw new SimulationExecutionException(
                    $"Simulation service returned {response.StatusCode}",
                    "UnknownError"
                );
            }

            // Getting response content
            var result = await response.Content.ReadFromJsonAsync<FastApiResponseDto>();

            if (result is null)
            {
                throw new SimulationExecutionException(
                    "Simulation service returned an empty response",
                    "EmptyResponseError"
                );
            }

            _logger.LogInformation("Simulation executed successfully");

            return result;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request to FastAPI failed");
            throw new SimulationExecutionException(
                "Failed to communicate with simulation engine",
                "CommunicationError"
            );
        }
        catch (SimulationExecutionException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during simulation execution");
            throw new SimulationExecutionException(
                "Unexpected error during simulation execution",
                "UnexpectedError"
            );
        }
    }

    // Private method for build request with composition and other parameters
    private static FastApiRequestDto BuildRequest(
        FuelCompositionDetails composition,
        SimulationParametersDetails parameters) => new()
    {
        MethaneMolarFraction = composition.MethaneMolarFraction,
        EthaneMolarFraction = composition.EthaneMolarFraction,
        PropaneMolarFraction = composition.PropaneMolarFraction,
        NButaneMolarFraction = composition.NButaneMolarFraction,
        WaterMolarFraction = composition.WaterMolarFraction,
        CarbonDioxideMolarFraction = composition.CarbonDioxideMolarFraction,
        HydrogenMolarFraction = composition.HydrogenMolarFraction,
        NitrogenMolarFraction = composition.NitrogenMolarFraction,
        FuelMassFlow = parameters.FuelMassFlow,
        FuelInputTemperature = parameters.FuelInputTemperature,
        AirInputTemperature = parameters.AirInputTemperature,
        PercentExcessAir = parameters.PercentExcessAir,
        LocalAtmosphericPressure = parameters.LocalAtmosphericPressure,
        LocalTemperature = parameters.LocalTemperature,
        RelativeHumidity = parameters.RelativeHumidity,
        GasTurbineEfficiency = parameters.GasTurbineEfficiency,
        ChimneyGasTemperature = parameters.ChimneyGasTemperature,
        PurgeLevel = parameters.PurgeLevel,
        HighSteamLevelPressure = parameters.HighSteamLevelPressure,
        MediumSteamLevelPressure = parameters.MediumSteamLevelPressure,
        LowSteamLevelPressure = parameters.LowSteamLevelPressure,
        HighSteamLevelTemperature = parameters.HighSteamLevelTemperature,
        MediumSteamLevelTemperature = parameters.MediumSteamLevelTemperature,
        LowSteamLevelTemperature = parameters.LowSteamLevelTemperature,
        HighSteamLevelFraction = parameters.HighSteamLevelFraction,
        MediumSteamLevelFraction = parameters.MediumSteamLevelFraction,
        HighSteamLevelEfficiency = parameters.HighSteamLevelEfficiency,
        MediumSteamLevelEfficiency = parameters.MediumSteamLevelEfficiency,
        LowSteamLevelEfficiency = parameters.LowSteamLevelEfficiency,
        ReductorGeneratorSetEfficiency = parameters.ReductorGeneratorSetEfficiency,
        PumpEfficiency = parameters.PumpEfficiency,
        EnginePumpEfficiency = parameters.EnginePumpEfficiency,
        PowerFactorPumpEfficiency = parameters.PowerFactorPumpEfficiency,
        CondenserOperationPressure = parameters.CondenserOperationPressure,
        RangeTemperatureCoolingWater = parameters.RangeTemperatureCoolingWater
    };
}
