using UnityEngine;

public class Node
{
    public Vector3Int position;
    public int g;
    public int h;
    public int f;
    public Node parent;

    public Node(Vector3Int position)
    {
        this.position = position;
        g = 0;
        h = 0;
        f = 0;
        parent = null;
    }
}

