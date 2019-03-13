using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static Grid instance;

    public Transform StartPosition, Ground, node;
    public float fNodeRadius, fNodeDiameter;
    public Vector3 offset;
    [Header("optinal whenn working in the engine")]
    public LayerMask layerMask;
    public bool draw = false;

    private Vector2 vGridWorldSize;
    private int iGridSizeX, iGridSizeY;
    //  public List<NodePosition> FinalPath;
    NodePosition[,] NodeArray;

    private void Awake()
    {
        instance = this;
        Vector3 box = Ground.GetComponent<Collider>().bounds.size;
        vGridWorldSize = new Vector2(box.x, box.z);
        fNodeDiameter = fNodeRadius * 2;
        iGridSizeX = Mathf.RoundToInt(vGridWorldSize.x / fNodeDiameter);
        iGridSizeY = Mathf.RoundToInt(vGridWorldSize.y / fNodeDiameter);
        CreateGrid();
    }



    public int MaxSize
    {
        get
        {
            return iGridSizeX * iGridSizeY;
        }
    }

    void CreateGrid()
    {
        NodeArray = new NodePosition[iGridSizeX, iGridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * vGridWorldSize.x / 2 - Vector3.forward * vGridWorldSize.y / 2;
        for (int x = 0; x < iGridSizeX; x++)
        {
            for (int y = 0; y < iGridSizeY; y++)
            {
                Vector3 worldPoint = bottomLeft + Vector3.right * (x * fNodeDiameter + fNodeRadius) + Vector3.forward * (y * fNodeDiameter + fNodeRadius);
                

                if (!Physics.CheckSphere(worldPoint, fNodeRadius, layerMask))
                {
                    NodePosition nodePosition = new NodePosition(true, worldPoint, x, y, true);
                    NodeArray[x, y] = nodePosition;
                    node.GetComponent<KnowNode>().setXY(x, y, true);
                }
                else
                {
                    NodePosition nodePosition = new NodePosition(true, worldPoint, x, y, false);
                    NodeArray[x, y] = nodePosition;
                    node.GetComponent<KnowNode>().setXY(x, y, false);
                }
                Instantiate(node, worldPoint + offset, Quaternion.identity);

            }
        }
    }

    public List<NodePosition> GetNeighboringNodes(NodePosition node)
    {
        List<NodePosition> NeighborList = new List<NodePosition>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.iGridX + x;
                int checkY = node.iGridY + y;

                if (checkX >= 0 && checkX < iGridSizeX && checkY >= 0 && checkY < iGridSizeY)
                    NeighborList.Add(NodeArray[checkX, checkY]);

            }
        }
   
        return NeighborList;
    }



    public NodePosition NodeFromWorldPoint(Vector3 a_vWorldPos)
    {
        float ixPos = (a_vWorldPos.x + vGridWorldSize.x / 2) / vGridWorldSize.x;
        float iyPos = (a_vWorldPos.z + vGridWorldSize.y / 2) / vGridWorldSize.y;
        ixPos = Mathf.Clamp01(ixPos);
        iyPos = Mathf.Clamp01(iyPos);
        int ix = Mathf.RoundToInt((iGridSizeX - 1) * ixPos);
        int iy = Mathf.RoundToInt((iGridSizeY - 1) * iyPos);
        return NodeArray[ix, iy];
    }
    public void updateNode(int x, int z, bool _walkable)
    {
        NodeArray[x, z].walkable = _walkable;
      
    }


    private void OnDrawGizmos()
    { 
        if (NodeArray != null && draw)
        {
            foreach (NodePosition n in NodeArray)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.yellow;
                if (!n.buildAble)
                    Gizmos.color = Color.black;

                Gizmos.DrawCube(n.vPosition + offset, Vector3.one * (fNodeDiameter - .1f));
             
            }
        }
    }
}
