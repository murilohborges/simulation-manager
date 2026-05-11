using Microsoft.EntityFrameworkCore;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.ValueObjects;
using System.Text.Json;

namespace SimulationManager.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options ) : base( options )
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Simulation> Simulations { get; set; }
    public DbSet<FuelComposition> FuelCompositions { get; set; }
    public DbSet<SimulationResult> SimulationResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Sets explicit the relationship 1:1
        modelBuilder.Entity<Simulation>()
            .HasOne(s => s.SimulationResult)
            .WithOne(r => r.Simulation)
            .HasForeignKey<SimulationResult>(r => r.SimulationId)
            .OnDelete(DeleteBehavior.Cascade); 

        // SPECIF CONFIGS
        // Convert created_at to timestamp
        modelBuilder.Entity<User>()
            .Property(s => s.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Simulation>()
            .Property(s => s.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<FuelComposition>()
            .Property(f => f.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<SimulationResult>()
            .Property(f => f.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        // UpdatedAt configs — nullable, no default
        modelBuilder.Entity<User>()
            .Property(u => u.UpdatedAt)
            .HasColumnType("timestamp")
            .IsRequired(false);

        modelBuilder.Entity<FuelComposition>()
            .Property(f => f.UpdatedAt)
            .HasColumnType("timestamp")
            .IsRequired(false);

        // Convert colums from longtext to json type
        modelBuilder.Entity<Simulation>()
            .Property(s => s.Parameters)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<SimulationParametersDetails>(v, (JsonSerializerOptions)null)
            )
            .HasColumnType("json");

        modelBuilder.Entity<FuelComposition>()
            .Property(f => f.Composition)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<FuelCompositionDetails>(v, (JsonSerializerOptions)null)
            )
            .HasColumnType("json")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<SimulationResult>()
            .Property(r => r.ResultJson)
            .HasColumnType("json")
            .HasCharSet("utf8mb4");

        // global configs
        // padrão global (snake_case)
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.Name));
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()));
            }

            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
            }
        }
    }

    private static string ToSnakeCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var result = new System.Text.StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            var c = input[i];

            if (char.IsUpper(c))
            {
                if (i > 0)
                    result.Append('_');

                result.Append(char.ToLower(c));
            }
            else
            {
                result.Append(c);
            }
        }

        return result.ToString();
    }
}
