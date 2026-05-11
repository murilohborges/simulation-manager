using SimulationManager.Domain.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimulationManager.Domain.Entities;

[Table("Users")]
public class User : IAuditableEntity
{
    public User()
    {
        FuelCompositions = new Collection<FuelComposition>();
        Simulations = new Collection<Simulation>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string? UserName { get; set; }

    [Required]
    [StringLength(80)]
    public string? Role { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public ICollection<FuelComposition>? FuelCompositions { get; set; }

    public ICollection<Simulation>? Simulations { get; set; }

}
