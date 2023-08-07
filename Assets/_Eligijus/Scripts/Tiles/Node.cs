
using UnityEngine;

[System.Serializable]
public class Node
{
    public ChunkData data;
    public Node left;
    public Node right;
    public Node parent;
    public Node(ChunkData data)
    {
        this.data = data;
    }
}