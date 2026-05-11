using SimulationManager.Domain.Interfaces;
using SimulationManager.Domain.ValueObjects;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SimulationManager.Domain.Entities;

[Table("FuelCompositions")]
public class FuelComposition : IAuditableEntity
{
    public FuelComposition()
    {
        Simulations = new Collection<Simulation>();
    }
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string? Name { get; set; }
    [Required]
    public FuelCompositionDetails Composition { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public int UserId { get; set; }

    [JsonIgnore]
    public User? User { get; set; }

    public ICollection<Simulation>? Simulations { get; set; }
}
