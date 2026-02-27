using Godot;

public static class CellularAutomata
{
    public static bool[,] Generate(int width, int height, float fillProbability, int smoothingIterations, int birthLimit, int deathLimit, int seed = -1)
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        if (seed != -1)
            rng.Seed = (ulong)seed;

        bool[,] grid = new bool[width, height];

        // Initialize grid with random values
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = rng.Randf() < fillProbability;
            }
        }

        // Smooth the grid
        for (int i = 0; i < smoothingIterations; i++)
        {
            grid = SmoothGrid(grid, birthLimit, deathLimit);
        }

        return grid;
    }

    private static bool[,] SmoothGrid(bool[,] grid, int birthLimit, int deathLimit)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        bool[,] newGrid = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int aliveNeighbors = CountAliveNeighbors(grid, x, y);

                if (grid[x, y])
                {
                    // Cell is alive
                    newGrid[x, y] = aliveNeighbors >= deathLimit;
                }
                else
                {
                    // Cell is dead
                    newGrid[x, y] = aliveNeighbors > birthLimit;
                }
            }
        }

        return newGrid;
    }

    private static int CountAliveNeighbors(bool[,] grid, int x, int y)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        int count = 0;

        for (int nx = x - 1; nx <= x + 1; nx++)
        {
            for (int ny = y - 1; ny <= y + 1; ny++)
            {
                if (nx == x && ny == y) continue;
                if (nx < 0 || nx >= width || ny < 0 || ny >= height) continue;

                if (grid[nx, ny])
                    count++;
            }
        }

        return count;
    }
}