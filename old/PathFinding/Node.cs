using UnityEngine;

public class Node : HeapItem<Node>
{
    public bool Walkable { get; set; }
    public int MovementPenalty { get; set; }
    public Vector3 WorldPosition { get; set; }
    
    public int GridPositionX { get; set; }
    public int GridPositionY { get; set; }

    /// <summary>
    /// Distance between node and start node
    /// </summary>
    public int GCost { get; set; }
    
    /// <summary>
    /// Distance between node and target node
    /// </summary>
    public int HCost { get; set; }

    /// <summary>
    /// Sum of GCost and HCost
    /// </summary>
    public int FCost => GCost + HCost;
    
    public Node Parent { get; set; }
    
    public Node(Vector3 worldPosition, int gridX, int gridY, bool walkable, int penalty)
    {
        WorldPosition = worldPosition;
        GridPositionX = gridX;
        GridPositionY = gridY;
        Walkable = walkable;
        MovementPenalty = penalty;
    }

    public int GetDistance(Node node)
    {
        int distX = Mathf.Abs(GridPositionX - node.GridPositionX);
        int distY = Mathf.Abs(GridPositionY - node.GridPositionY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }

    public override int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);

        if (compare == 0)
            compare = HCost.CompareTo(other.HCost);

        return -compare;
    }
}
