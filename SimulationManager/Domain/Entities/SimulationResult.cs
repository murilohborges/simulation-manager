using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimulationManager.Domain.Entities;

[Table("SimulationResults")]
public class SimulationResult
{
    [Key]
    public int Id { get; set; }
    public string? ResultJson { get; set; }
    public DateTime CreatedAt { get; set; }

    public int SimulationId { get; set; }

    [JsonIgnore]
    public Simulation? Simulation { get; set; }

}
