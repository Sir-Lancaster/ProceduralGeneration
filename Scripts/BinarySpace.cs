using Godot;

public class BinarySpace
{
    private int _maxDepth; 
    private int _minSize; 
    private BSPNode _root;

    // Builds and returns the tree
    public static BSPNode BuildBSP(Rect2I region, int depth)
    {
        // something
    }

    // Walks the tree and stores rooms on leaves, no return needed
    public static void PlaceRooms(BSPNode node)
    {
        return; 
    }

    // Walks the tree and stores corridors on internal nodes, no return needed
    public static void ConnectRegions(BSPNode node)
    {
        return;
    }

    public void Generate(int MaxDepth, int MinSize, int width, int height)
    {
        return;
    }
}