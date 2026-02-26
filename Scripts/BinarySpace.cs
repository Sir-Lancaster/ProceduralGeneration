using Godot;
using System;

public class BinarySpace
{
    private int _maxDepth; 
    private int _minSize; 
    private int _seed; 
    private Random _random;
    private float _splitChance;
    private BSPNode _root;
    public BSPNode Root => _root;

    // Builds and returns the tree
    private BSPNode BuildBSP(Rect2I region, int depth)
    {
        // Create the node.
        BSPNode node = new BSPNode(region);

        // Stopping conditions.
        if (depth >= _maxDepth || region.Size.X < _minSize * 2 || region.Size.Y < _minSize * 2)
        {
            return node;
        }

        // Check split chance for random early exit condition.
        if (_random.NextDouble() > _splitChance)
        {
            return node;
        }

        // Determine the direction of the split.
        bool splitHorizontally = region.Size.Y > region.Size.X;

        // The split & recursion.
        if (splitHorizontally)
        {
            int splitY = _random.Next(
                region.Position.Y + _minSize,
                region.Position.Y + region.Size.Y - _minSize
            );
            
            Rect2I topRegion = new Rect2I(
                region.Position.X,
                region.Position.Y,
                region.Size.X,
                splitY - region.Position.Y
            );

            Rect2I bottomRegion = new Rect2I(
                region.Position.X,
                splitY,
                region.Size.X,
                region.Size.Y - (splitY - region.Position.Y)
            );
            node.Left = BuildBSP(topRegion, depth + 1);
            node.Right = BuildBSP(bottomRegion, depth + 1);
        }
        
        else
        {
            int splitX = _random.Next(
                region.Position.X + _minSize,
                region.Position.X + region.Size.X - _minSize
            );

            Rect2I leftRegion = new Rect2I(
                region.Position.X,
                region.Position.Y,
                splitX - region.Position.X,
                region.Size.Y
            );

            Rect2I rightRegion = new Rect2I(
                splitX,
                region.Position.Y,
                region.Size.X - (splitX - region.Position.X),
                region.Size.Y
            );
            node.Left = BuildBSP(leftRegion, depth + 1);
            node.Right = BuildBSP(rightRegion, depth + 1);
        }
        
        return node;
    }

    // Walks the tree and stores rooms on leaves
    private void PlaceRooms(BSPNode node)
    {
        
    }

    // Walks the tree and stores corridors on internal nodes
    private void ConnectRegions(BSPNode node)
    {
        
    }

    public void Generate(int maxDepth, int minSize, int width, int height, int seed, float splitChance)
    {
        // Initialize parameters.
        _maxDepth = maxDepth;
        _minSize = minSize;
        _seed = seed;
        _splitChance = splitChance;
        _random = new Random(_seed);

        // Create the starting region.
        Rect2I startingRegion = new Rect2I(0, 0, width, height);

        // Call functions to build the tree and walk the algorithm.
        _root = BuildBSP(startingRegion, 0);
        PlaceRooms(_root);
        ConnectRegions(_root);
    }
}