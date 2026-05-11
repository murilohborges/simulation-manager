using SimulationManager.Domain.ValueObjects;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimulationManager.Domain.Entities;

[Table("Simulations")]
public class Simulation
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string? Status { get; set; }
    public SimulationParametersDetails Parameters { get; set; }
    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    public int FuelCompositionId { get; set; }

    [JsonIgnore]
    public FuelComposition? FuelComposition { get; set; }

    public SimulationResult? SimulationResult { get; set; }

}
