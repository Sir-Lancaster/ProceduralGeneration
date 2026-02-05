using Godot;

public static class PerlinNoise
{
    // Copilot showed me both the 8 and 12 vector implementation, I like the 8 vector version.
    private static readonly Vector2[] Gradients =
    [
        new Vector2(1, 0),  // East.
        new Vector2(-1, 0), // West.
        new Vector2(0, 1),  // North.
        new Vector2(0, -1), // South.
        new Vector2(1, 1).Normalized(),  // North East.
        new Vector2(-1, 1).Normalized(), // North West.
        new Vector2(1, -1).Normalized(), // South East.
        new Vector2(-1, -1).Normalized() // South West.
    ];

    public static float PerlinNoise2D(float x, float y, int[] permTable)
    {
        // Find grid square corners
        int x0 = (int)Mathf.Floor(x);  // left edge
        int y0 = (int)Mathf.Floor(y);  // bottom edge
        int x1 = x0 + 1;
        int y1 = y0 + 1;

        // Calculate distance vectors
        float dx = x - x0;
        float dy = y - y0;

        // Look up gradients using permutation table
        int p00 = permTable[(permTable[x0 & 255] + y0) & 255];
        int p01 = permTable[(permTable[x0 & 255] + y1) & 255];
        int p10 = permTable[(permTable[x1 & 255] + y0) & 255];
        int p11 = permTable[(permTable[x1 & 255] + y1) & 255];

        // Record gradient indexes.
        int gradientIndex00 = p00 % Gradients.Length;
        int gradientIndex01 = p01 % Gradients.Length;
        int gradientIndex10 = p10 % Gradients.Length;
        int gradientIndex11 = p11 % Gradients.Length;

        // look up gradients.
        Vector2 g00 = Gradients[gradientIndex00];
        Vector2 g01 = Gradients[gradientIndex01];
        Vector2 g10 = Gradients[gradientIndex10];
        Vector2 g11 = Gradients[gradientIndex11];

        // Calculate dot products.
        float d00 = g00.Dot(new Vector2(dx, dy));
        float d01 = g01.Dot(new Vector2(dx, dy -1));
        float d10 = g10.Dot(new Vector2(dx - 1, dy));
        float d11 = g11.Dot(new Vector2(dx - 1, dy - 1));

        // Smooth interpolate
        float sX = Smoothstep(dx);
        float sY = Smoothstep(dy);

        float ix0 = Lerp(d00, d10, sX);
        float ix1 = Lerp(d01, d11, sX);

        float result = Lerp(ix0, ix1, sY);

        return result;
    }

    public static int[] GeneratePermutationTable(int seed)
    {
        RandomNumberGenerator rng = new RandomNumberGenerator
        {
            Seed = (ulong)seed
        };

        int[] PermutationTable_init = new int[256];

        // Create array [0, 1, 2, ..., 255]
        for (int i = 0; i < 256; i++)
        {
            PermutationTable_init[i] = i;
        }
        
        // Shuffle it using the seed
        for (int i = 255; i > 0; i--)
        {
            int j = rng.RandiRange(0,i);

            int temp = PermutationTable_init[i];
            PermutationTable_init[i] = PermutationTable_init[j];
            PermutationTable_init[j] = temp;
        }

        // Duplicate to 512 length
        int[] PermTable = new int[512];
        for (int i = 0; i < 512; i++)
        {
            PermTable[i] = PermutationTable_init[i & 255];
        }
        
        return PermTable;
    }

    public static float FractalNoise2D(float x, float y, int[] permTable, int octaves, float persistence, float lacunarity, float scale)
    {
        float total = 0f;
        float amplitude = 1f;
        float frequency = scale;
        float maxValue = 0f;  // For normalization

        for (int i = 0; i < octaves; i++)
        {
            // Sample Perlin noise at current frequency
            float sampleX = x * frequency;
            float sampleY = y * frequency;
            
            float noiseValue = PerlinNoise2D(sampleX, sampleY, permTable);
            
            // Add this octave's contribution
            total += noiseValue * amplitude;
            
            // Track max possible value for normalization
            maxValue += amplitude;
            
            // Prepare for next octave
            amplitude *= persistence;  // Reduce amplitude
            frequency *= lacunarity;   // Increase frequency
        }

        // Normalize to [-1, 1] range
        return total / maxValue;
    }

    // Helper functions.
    private static float Smoothstep(float t)
    {
        return t * t * (3 - 2 * t);
    }

    private static float Lerp(float a, float b, float t)
    {
        return a + (b - a) * t;
    }
}