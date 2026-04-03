using System.Security.Cryptography;

namespace Bank.Application.Validators.Shared;

/// <summary>
/// Utility for generating random values using cryptographic randomness
/// </summary>
public static class RandomGenerator
{
    /// <summary>
    /// Generates a random boolean based on probability
    /// </summary>
    /// <param name="probability">Probability of returning true (0.0 to 1.0)</param>
    /// <returns>Random boolean</returns>
    public static bool GenerateBoolean(double probability = 0.5)
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        var randomValue = Math.Abs(BitConverter.ToInt32(randomBytes, 0)) / (double)int.MaxValue;
        return randomValue < probability;
    }

    /// <summary>
    /// Generates a random number within a specified range
    /// </summary>
    /// <param name="min">Minimum value (inclusive)</param>
    /// <param name="max">Maximum value (exclusive)</param>
    /// <returns>Random number</returns>
    public static int GenerateNumber(int min, int max)
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[4];
        rng.GetBytes(randomBytes);
        var randomValue = Math.Abs(BitConverter.ToInt32(randomBytes, 0));
        return min + (randomValue % (max - min));
    }
}
