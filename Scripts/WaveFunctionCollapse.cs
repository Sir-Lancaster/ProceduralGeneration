using Godot;
using System.Collections.Generic;
using System.Numerics;

public class WaveFunctionCollapse
{
    private ulong[,] _grid;
    private int _width;
    private int _height;

    // [tileId, direction] = bitmask of allowed neighbors
    // Directions: 0=North, 1=East, 2=South, 3=West
    int[] dx = { 0, 1, 0, -1 };
    int[] dy = { -1, 0, 1, 0 };
    private static readonly ulong[,] _constraintRules = BuildConstraints();

    private static ulong[,] BuildConstraints()
    {
        // Each tile's connections: [N, E, S, W]
        bool[,] connections = new bool[16, 4]
        {
            // N      E      S      W
            { false, false, false, false }, // 0  Blank
            { true,  false, false, false }, // 1  Dead end up
            { false, true,  false, false }, // 2  Dead end right
            { false, false, true,  false }, // 3  Dead end down
            { false, false, false, true  }, // 4  Dead end left
            { true,  true,  true,  true  }, // 5  Four-way
            { true,  true,  true,  false }, // 6  T (N,E,S)
            { true,  true,  false, true  }, // 7  T (N,E,W)
            { true,  false, true,  true  }, // 8  T (N,S,W)
            { false, true,  true,  true  }, // 9  T (E,S,W)
            { false, true,  true,  false }, // 10 Elbow (E,S)
            { false, true,  false, true  }, // 11 Straight horizontal
            { false, false, true,  true  }, // 12 Elbow (S,W)
            { true,  true,  false, false }, // 13 Elbow (N,E)
            { true,  false, true,  false }, // 14 Straight vertical
            { true,  false, false, true  }, // 15 Elbow (N,W)
        };

        ulong[,] rules = new ulong[16, 4];

        for (int tile = 0; tile < 16; tile++)
        {
            for (int dir = 0; dir < 4; dir++)
            {
                // Opposite direction index
                int opposite = (dir + 2) % 4;
                ulong mask = 0;

                for (int neighbor = 0; neighbor < 16; neighbor++)
                {
                    // Tiles are compatible if both connect or both don't connect on shared edge
                    if (connections[tile, dir] == connections[neighbor, opposite])
                        mask |= (1UL << neighbor);
                }

                rules[tile, dir] = mask;
            }
        }

        return rules;
    }

    public WaveFunctionCollapse(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public bool Generate(float[] weights)
    {
        InitializeGrid(weights);
        
        while (true)
        {
            // Find the cell with minimum entropy
            Vector2I cell = FindMinimumEntropyCell();

            // No uncollapsed cells, we're done!
            if (cell.X == -1 && cell.Y == -1)
                return true;

            // Collapse the cell to a single tile
            int chosenTile = WeightedRandomBit(_grid[cell.Y, cell.X], weights);
            _grid[cell.Y, cell.X] = 1UL << chosenTile;
            
            Queue<Vector2I> propagationQueue = new Queue<Vector2I>();
            propagationQueue.Enqueue(cell);

            while (propagationQueue.Count > 0)
            {
                Vector2I currentCell = propagationQueue.Dequeue();
                
                for (int dir = 0; dir < 4; dir++)
                {
                    int nx = currentCell.X + dx[dir];
                    int ny = currentCell.Y + dy[dir];

                    if (nx < 0 || nx >= _width || ny < 0 || ny >= _height)
                        continue;

                    ulong oldPossibilities = _grid[ny, nx];

                    ulong allowedNeighbors = 0;
                    for (int t = 0; t < 16; t++)
                    {
                        if ((_grid[currentCell.Y, currentCell.X] & (1UL << t)) != 0)
                            allowedNeighbors |= _constraintRules[t, dir];
                    }

                    _grid[ny, nx] &= allowedNeighbors;

                    if (_grid[ny, nx] != oldPossibilities)
                    {
                        if (_grid[ny, nx] == 0)
                            return false; // contradiction

                        propagationQueue.Enqueue(new Vector2I(nx, ny));
                    }
                }
            }
        }
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
                _grid[y,x] = startingMask;
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

    public int GetTile(int x, int y)
    {
        return BitOperations.TrailingZeroCount(_grid[y, x]);
    }
}