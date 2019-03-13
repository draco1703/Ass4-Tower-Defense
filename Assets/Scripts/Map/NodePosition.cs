using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodePosition : IHeapItem<NodePosition>
{
    public int iGridX;
    public int iGridY;
    public bool walkable;
    public bool buildAble;
    public Vector3 vPosition;
    public NodePosition ParentNode;
    public int gCost;
    public int hCost;
    int heapIndex;

    public int fCost { get { return gCost + hCost; } }

    public NodePosition(bool a_walkable, Vector3 a_vPos, int a_igridX, int a_igridY, bool _buildable)
    {
        walkable = a_walkable;
        vPosition = a_vPos + Vector3.up;
        iGridX = a_igridX;
        iGridY = a_igridY;
        buildAble = _buildable;
    }

    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int CompareTo(NodePosition nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
