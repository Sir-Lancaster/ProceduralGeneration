using Godot;
using System.Collections.Generic;
using System.Numerics;

public class WaveFunctionCollapse
{
    private ulong[,] _grid;
    private int _width;
    private int _height;

    public WaveFunctionCollapse(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public bool Generate(float[] weights)
    {
        InitializeGrid(weights);

        Vector2I cell = FindMinimumEntropyCell();
        
    }

    private void InitializeGrid(float[] weights)
    {
        _grid = new ulong[_height, _width];
        ulong startingMask = 0;
        for (int i = 0; i < 16; i++)
        {
            if (weights[i] > 0)
            {
                startingMask |= 1UL << i;
            }
        }

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                _grid[x,y] = startingMask;
            }
        }
    }

    private Vector2I FindMinimumEntropyCell()
    {
        int minEntropy = int.MaxValue;
        List<Vector2I> candidates = new List<Vector2I>();

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                int entropy = BitOperations.PopCount(_grid[y, x]);

                if (entropy <= 1) continue; // collapsed or contradiction, skip

                if (entropy < minEntropy)
                {
                    minEntropy = entropy;
                    candidates.Clear();
                    candidates.Add(new Vector2I(x, y));
                }
                else if (entropy == minEntropy)
                {
                    candidates.Add(new Vector2I(x, y));
                }
            }
        }

        // No uncollapsed cells found
        if (candidates.Count == 0)
            return new Vector2I(-1, -1); // signal that we're done

        // Return random candidate
        return candidates[GD.RandRange(0, candidates.Count - 1)];
    }

    private int WeightedRandomBit(ulong superposition, float[] weights)
    {
        float totalWeight = 0f;

        // Sum weights of all possible tiles in this cell's superposition
        for (int i = 0; i < 16; i++)
        {
            if ((superposition & (1UL << i)) != 0)
                totalWeight += weights[i];
        }

        // Pick a random point along that total weight
        float random = (float)GD.RandRange(0.0, totalWeight);

        // Walk through bits until we pass the random point
        float cumulative = 0f;
        for (int i = 0; i < 16; i++)
        {
            if ((superposition & (1UL << i)) != 0)
            {
                cumulative += weights[i];
                if (random <= cumulative)
                    return i; // return the tile index
            }
        }

        return -1; // should never reach here
    }
}