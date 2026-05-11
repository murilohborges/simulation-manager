using SimulationManager.Infrastructure.Persistence;
using SimulationManager.Domain.Entities;
using SimulationManager.Domain.ValueObjects;

namespace SimulationManager.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        bool hasUsers = context.Users.Any();
        bool hasFuels = context.FuelCompositions.Any();

        if (hasUsers && hasFuels)
            return;

        var now = DateTime.UtcNow;

        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            User user = null;

            if (!hasUsers)
            {
                user = new User
                {
                    UserName = "murilo",
                    Role = "admin",
                    CreatedAt = now
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            else
            {
                user = context.Users.First();
            }

            if (!hasFuels)
            {
                var fuel = new FuelComposition
                {
                    Name = "Natural Gas A",
                    UserId = user.Id,
                    CreatedAt = now,
                    Composition = new FuelCompositionDetails
                    {
                        MethaneMolarFraction = 90,
                        NitrogenMolarFraction = 5,
                        CarbonDioxideMolarFraction = 5,
                        EthaneMolarFraction = 0,
                        PropaneMolarFraction = 0,
                        NButaneMolarFraction = 0,
                        WaterMolarFraction = 0,
                        HydrogenMolarFraction = 0
                    }
                };
                context.FuelCompositions.Add(fuel);
                await context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}