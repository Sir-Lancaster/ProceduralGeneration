using Godot;

public class BinarySpace
{
    // variables tbd

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
}